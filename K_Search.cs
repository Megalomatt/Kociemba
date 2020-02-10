using System;

namespace Kociemba
{
    public class Search
    {
        internal static int[] ax = new int[31]; // The axis of the move
        internal static int[] po = new int[31]; // The power of the move

        internal static int[] flip = new int[31]; // phase1 coordinates
        internal static int[] twist = new int[31];
        internal static int[] slice = new int[31];

        internal static int[] parity = new int[31]; // phase2 coordinates
        internal static int[] URFtoDLF = new int[31];
        internal static int[] FRtoBR = new int[31];
        internal static int[] URtoUL = new int[31];
        internal static int[] UBtoDF = new int[31];
        internal static int[] URtoDF = new int[31];

        internal static int[] minDistPhase1 = new int[31]; // IDA* distance do goal estimations
        internal static int[] minDistPhase2 = new int[31];
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // generate the solution string from the array data
        internal static string solutionToString(int length)
        {
            string s = "";
            for (int i = 0; i < length; i++)
            {
                switch (ax[i])
                {
                    case 0:
                        s += "U";
                        break;
                    case 1:
                        s += "R";
                        break;
                    case 2:
                        s += "F";
                        break;
                    case 3:
                        s += "D";
                        break;
                    case 4:
                        s += "L";
                        break;
                    case 5:
                        s += "B";
                        break;
                }
                switch (po[i])
                {
                    case 1:
                        s += " ";
                        break;
                    case 2:
                        s += "2 ";
                        break;
                    case 3:
                        s += "' ";
                        break;

                }
            }
            return s;
        }

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // generate the solution string from the array data including a separator between phase1 and phase2 moves
        internal static string solutionToString(int length, int depthPhase1)
        {
            string s = "";
            for (int i = 0; i < length; i++)
            {
                switch (ax[i])
                {
                    case 0:
                        s += "U";
                        break;
                    case 1:
                        s += "R";
                        break;
                    case 2:
                        s += "F";
                        break;
                    case 3:
                        s += "D";
                        break;
                    case 4:
                        s += "L";
                        break;
                    case 5:
                        s += "B";
                        break;
                }
                switch (po[i])
                {
                    case 1:
                        s += " ";
                        break;
                    case 2:
                        s += "2 ";
                        break;
                    case 3:
                        s += "' ";
                        break;

                }
                if (i == depthPhase1 - 1)
                {
                    s += ". ";
                }
            }
            return s;
        }

        /**
         * Computes the solver string for a given cube.
         * 
         * @param facelets
         *          is the cube definition string, see {@link Facelet} for the format.
         * 
         * @param maxDepth
         *          defines the maximal allowed maneuver length. For random cubes, a maxDepth of 21 usually will return a
         *          solution in less than 0.5 seconds. With a maxDepth of 20 it takes a few seconds on average to find a
         *          solution, but it may take much longer for specific cubes.
         * 
         *@param timeOut
         *          defines the maximum computing time of the method in seconds. If it does not return with a solution, it returns with
         *          an error code.
         * 
         * @param useSeparator
         *          determines if a " . " separates the phase1 and phase2 parts of the solver string like in F' R B R L2 F .
         *          U2 U D for example.<br>
         * @return The solution string or an error code:<br>
         *         Error 1: There is not exactly one facelet of each colour<br>
         *         Error 2: Not all 12 edges exist exactly once<br>
         *         Error 3: Flip error: One edge has to be flipped<br>
         *         Error 4: Not all corners exist exactly once<br>
         *         Error 5: Twist error: One corner has to be twisted<br>
         *         Error 6: Parity error: Two corners or two edges have to be exchanged<br>
         *         Error 7: No solution exists for the given maxDepth<br>
         *         Error 8: Timeout, no solution within given time
         */
        public static string solution(string facelets, out string info, int maxDepth = 22, long timeOut = 6000, bool useSeparator = false )
        {

            DateTime startTime = DateTime.Now;
            info = "";
      
                     
            int s;
            // +++++++++++++++++++++check for wrong input +++++++++++++++++++++++++++++
            int[] count = new int[6];
            try
            {
                for (int i = 0; i < 54; i++)
                {
                    count[(int)CubeColor.Parse(typeof(CubeColor), facelets.Substring(i, 1))]++;
                }
            }
            catch (Exception)
            {
                return "Error 1";
            }
            for (int i = 0; i < 6; i++)
            {
                if (count[i] != 9)
                {
                    return "Error 1";
                }
            }

            FaceCube fc = new FaceCube(facelets);
            CubieCube cc = fc.toCubieCube();
            if ((s = cc.verify()) != 0)
            {
                return "Error " + Math.Abs(s);
            }
            // +++++++++++++++++++++++ initialization +++++++++++++++++++++++++++++++++
            string currentTime = "[ a: " + String.Format(@"{0:mm\:ss\.ffff}", (DateTime.Now - startTime)) + " ] ";
            CoordCube c = new CoordCube(cc, startTime, currentTime, out info);
            //return "lol";

            po[0] = 0;
            ax[0] = 0;
            flip[0] = c.flip;
            twist[0] = c.twist;
            parity[0] = c.parity;
            slice[0] = c.FRtoBR / 24;
            URFtoDLF[0] = c.URFtoDLF;
            FRtoBR[0] = c.FRtoBR;
            URtoUL[0] = c.URtoUL;
            UBtoDF[0] = c.UBtoDF;
            minDistPhase1[1] = 1; // else failure for depth=1, n=0
            int mv = 0, n = 0;
            bool busy = false;
            int depthPhase1 = 1;
            long tStart = DateTimeHelper.CurrentUnixTimeMillis();
           
            // +++++++++++++++++++ Main loop ++++++++++++++++++++++++++++++++++++++++++
            do
            {
                do
                {
                    if ((depthPhase1 - n > minDistPhase1[n + 1]) && !busy)
                    {

                        if (ax[n] == 0 || ax[n] == 3) // Initialize next move
                        {
                            ax[++n] = 1;
                        }
                        else
                        {
                            ax[++n] = 0;
                        }
                        po[n] = 1;
                    }
                    else if (++po[n] > 3)
                    {
                        do
                        { // increment axis
                            if (++ax[n] > 5)
                            {

                                if (DateTimeHelper.CurrentUnixTimeMillis() - tStart > timeOut << 10)
                                {
                                    return "Error 8";
                                }

                                if (n == 0)
                                {
                                    if (depthPhase1 >= maxDepth)
                                    {
                                        return "Error 7";
                                    }
                                    else
                                    {
                                        depthPhase1++;
                                        ax[n] = 0;
                                        po[n] = 1;
                                        busy = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    n--;
                                    busy = true;
                                    break;
                                }

                            }
                            else
                            {
                                po[n] = 1;
                                busy = false;
                            }
                        } while (n != 0 && (ax[n - 1] == ax[n] || ax[n - 1] - 3 == ax[n]));
                    }
                    else
                    {
                        busy = false;
                    }
                } while (busy);

                // +++++++++++++ compute new coordinates and new minDistPhase1 ++++++++++
                // if minDistPhase1 =0, the H subgroup is reached
                mv = 3 * ax[n] + po[n] - 1;
                flip[n + 1] = CoordCube.flipMove[flip[n],mv];
                twist[n + 1] = CoordCube.twistMove[twist[n],mv];
                slice[n + 1] = CoordCube.FRtoBR_Move[slice[n] * 24,mv] / 24;
                minDistPhase1[n + 1] = Math.Max(CoordCube.getPruning(CoordCube.Slice_Flip_Prun, CoordCube.N_SLICE1 * flip[n + 1] + slice[n + 1]), CoordCube.getPruning(CoordCube.Slice_Twist_Prun, CoordCube.N_SLICE1 * twist[n + 1] + slice[n + 1]));
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                if (minDistPhase1[n + 1] == 0 && n >= depthPhase1 - 5)
                {
                    minDistPhase1[n + 1] = 10; // instead of 10 any value >5 is possible
                    if (n == depthPhase1 - 1 && (s = totalDepth(depthPhase1, maxDepth)) >= 0)
                    {
                        if (s == depthPhase1 || (ax[depthPhase1 - 1] != ax[depthPhase1] && ax[depthPhase1 - 1] != ax[depthPhase1] + 3))
                        {
                            if (facelets == "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB")
                            {
                                info = "Already Solved";
                                return "";
                            }
                            return useSeparator ? solutionToString(s, depthPhase1) : solutionToString(s);
                        }
                    }

                }
            } while (true);
        }

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Apply phase2 of algorithm and return the combined phase1 and phase2 depth. In phase2, only the moves
        // U,D,R2,F2,L2 and B2 are allowed.
        internal static int totalDepth(int depthPhase1, int maxDepth)
        {
            int mv = 0, d1 = 0, d2 = 0;
            int maxDepthPhase2 = Math.Min(10, maxDepth - depthPhase1); // Allow only max 10 moves in phase2
            for (int i = 0; i < depthPhase1; i++)
            {
                mv = 3 * ax[i] + po[i] - 1;
                URFtoDLF[i + 1] = CoordCube.URFtoDLF_Move[URFtoDLF[i], mv];
                FRtoBR[i + 1] = CoordCube.FRtoBR_Move[FRtoBR[i], mv];
                parity[i + 1] = CoordCube.parityMove[parity[i]][mv];
            }

            if ((d1 = CoordCube.getPruning(CoordCube.Slice_URFtoDLF_Parity_Prun, (CoordCube.N_SLICE2 * URFtoDLF[depthPhase1] + FRtoBR[depthPhase1]) * 2 + parity[depthPhase1])) > maxDepthPhase2)
            {
                return -1;
            }

            for (int i = 0; i < depthPhase1; i++)
            {
                mv = 3 * ax[i] + po[i] - 1;
                URtoUL[i + 1] = CoordCube.URtoUL_Move[URtoUL[i], mv];
                UBtoDF[i + 1] = CoordCube.UBtoDF_Move[UBtoDF[i], mv];
            }
            URtoDF[depthPhase1] = CoordCube.MergeURtoULandUBtoDF[URtoUL[depthPhase1],UBtoDF[depthPhase1]];

            if ((d2 = CoordCube.getPruning(CoordCube.Slice_URtoDF_Parity_Prun, (CoordCube.N_SLICE2 * URtoDF[depthPhase1] + FRtoBR[depthPhase1]) * 2 + parity[depthPhase1])) > maxDepthPhase2)
            {
                return -1;
            }

            if ((minDistPhase2[depthPhase1] = Math.Max(d1, d2)) == 0) // already solved
            {
                return depthPhase1;
            }

            // now set up search

            int depthPhase2 = 1;
            int n = depthPhase1;
            bool busy = false;
            po[depthPhase1] = 0;
            ax[depthPhase1] = 0;
            minDistPhase2[n + 1] = 1; // else failure for depthPhase2=1, n=0
                                      // +++++++++++++++++++ end initialization +++++++++++++++++++++++++++++++++
            do
            {
                do
                {
                    if ((depthPhase1 + depthPhase2 - n > minDistPhase2[n + 1]) && !busy)
                    {

                        if (ax[n] == 0 || ax[n] == 3) // Initialize next move
                        {
                            ax[++n] = 1;
                            po[n] = 2;
                        }
                        else
                        {
                            ax[++n] = 0;
                            po[n] = 1;
                        }
                    }
                    else if ((ax[n] == 0 || ax[n] == 3) ? (++po[n] > 3) : ((po[n] = po[n] + 2) > 3))
                    {
                        do
                        { // increment axis
                            if (++ax[n] > 5)
                            {
                                if (n == depthPhase1)
                                {
                                    if (depthPhase2 >= maxDepthPhase2)
                                    {
                                        return -1;
                                    }
                                    else
                                    {
                                        depthPhase2++;
                                        ax[n] = 0;
                                        po[n] = 1;
                                        busy = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    n--;
                                    busy = true;
                                    break;
                                }

                            }
                            else
                            {
                                if (ax[n] == 0 || ax[n] == 3)
                                {
                                    po[n] = 1;
                                }
                                else
                                {
                                    po[n] = 2;
                                }
                                busy = false;
                            }
                        } while (n != depthPhase1 && (ax[n - 1] == ax[n] || ax[n - 1] - 3 == ax[n]));
                    }
                    else
                    {
                        busy = false;
                    }
                } while (busy);
                // +++++++++++++ compute new coordinates and new minDist ++++++++++
                mv = 3 * ax[n] + po[n] - 1;

                URFtoDLF[n + 1] = CoordCube.URFtoDLF_Move[URFtoDLF[n], mv];
                FRtoBR[n + 1] = CoordCube.FRtoBR_Move[FRtoBR[n],mv];
                parity[n + 1] = CoordCube.parityMove[parity[n]][mv];
                URtoDF[n + 1] = CoordCube.URtoDF_Move[URtoDF[n], mv];

                minDistPhase2[n + 1] = Math.Max(CoordCube.getPruning(CoordCube.Slice_URtoDF_Parity_Prun, (CoordCube.N_SLICE2 * URtoDF[n + 1] + FRtoBR[n + 1]) * 2 + parity[n + 1]), CoordCube.getPruning(CoordCube.Slice_URFtoDLF_Parity_Prun, (CoordCube.N_SLICE2 * URFtoDLF[n + 1] + FRtoBR[n + 1]) * 2 + parity[n + 1]));
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            } while (minDistPhase2[n + 1] != 0);
            return depthPhase1 + depthPhase2;
        }
    }




//---------------------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2020 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to replace calls to Java's System.currentTimeMillis with the C# equivalent.
//	Unix time is defined as the number of seconds that have elapsed since midnight UTC, 1 January 1970.
//---------------------------------------------------------------------------------------------------------
internal static class DateTimeHelper
    {
        private static readonly System.DateTime Jan1st1970 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        public static long CurrentUnixTimeMillis()
        {
            return (long)(System.DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}