﻿using System;
using System.Collections.Generic;
using CommNet;
using System.Linq;

namespace RealAntennas
{
    public class RealAntennasTools : object
    {
        public static string VesselWalk(RACommNetwork net, string ModTag="[RealAntennas] ")
        {
            string res = string.Format(ModTag + "VesselWalk()\n");
            res += string.Format("FlightData has {0} vessels.\n", FlightGlobals.Vessels.Count);
            foreach (Vessel v in FlightGlobals.Vessels)
            {
                if (v.Connection == null)
                {
                    res += string.Format("Vessel {0} has a null connection.\n", v);
                    continue;
                }
                CommNetVessel cv = v.Connection;
                if (cv.Comm == null)
                {
                    res += string.Format("Vessel {0} with CommNetVessel {1} has null CommNode.\n", v, cv);
                    continue;
                }
                CommNode cn = cv.Comm;
                res += string.Format("Vessel {0} with CommNetVessel {1} has CommNode {2}\n", v, cv, cn);
                List<RealAntenna> updatedlist = v.FindPartModulesImplementing<RealAntenna>();
                foreach (RealAntenna ra in updatedlist)
                {
                    res += string.Format("... Contains realAntenna part {0} / {1}.  CN antennaPower: {2}.  Part antennaPower: {3}\n",
                        ra.part, ra.GetInfo(), cn.antennaTransmit.power, ra.antennaPower);
                }
            }
            return res;
        }

        public static string DumpAction(Action e)
        {
            string res = "";
            if (e != null)
            {
                List<Delegate> delegates = e.GetInvocationList().ToList();
                foreach (Delegate dgel in delegates)
                {
                    res += string.Format("Delegate: {0} / {1}\n", e.Target, e.Method);
                }
            }
            return res;
        }

        public static string DumpLink(CommLink link)
        {
            return string.Format("A/B/Both CanRelay: {0}/{1}/{2}\n", link.aCanRelay, link.bCanRelay, link.bothRelay) +
                string.Format("StrengthAR/BR/RR: {0}/{1}/{2}\n", link.strengthAR, link.strengthBR, link.strengthRR) +
                string.Format("Best signal: {0}", link.GetBestSignal()) +
                string.Format("  Cost: {0}\n", link.cost) +
                string.Format("Start: {0}\n", link.start) +
                string.Format("End: {0}\n", link.end) +
                string.Format("GetSignalStrength(start) / (end) / (no relays) / (both relays): {0}/{1}/{2}/{3}\n",
                                    link.GetSignalStrength(link.start),
                                    link.GetSignalStrength(link.end),
                                    link.GetSignalStrength(false, false),
                                    link.GetSignalStrength(true, true)) +
                string.Format("signalStrength: {0}", link.signalStrength);

            /* Some sample results:
            [LOG 13:25:45.815] [RealAntennasCommNetwork] [Trace] Link: Kerbin: Mesa South -to- RA-1-CS16 : 150727254.72 (Green)
            [LOG 13:25:45.815] [RealAntennasCommNetwork] [Trace] A/B/Both CanRelay: True/False/False
            StrengthAR/BR/RR: 0.609718471365098/0/0
            Best signal: 0.609718471365098  Cost: 150727254.721891
            Start: Node: Kerbin: Mesa South Links=3 : Home  Control  MultiHop : RealAntennas Gain:40dBi TxP:60dBm BW:10000KHz Draw:60dBm Coding:12dB
            End: Node: RA-1-CS16 Links=6 :   : RealAntennas Gain:6dBi TxP:30dBm BW:10000KHz Draw:33dBm Coding:1dB
            GetSignalStrength(start): 0
            GetSignalStrength(end): 0.609718471365098
            GetSignalStrength(no relays): 0

            Note the different Strength fields based on A/B/Both Relay state.  So... there can be a notion of direction?
            */
        }
    }
}