using System;

namespace Kociemba
{
    internal class CoordCube
    {

        internal const short N_TWIST = 2187; // 3^7 possible corner orientations
        internal const short N_FLIP = 2048; // 2^11 possible edge flips
        internal const short N_SLICE1 = 495; // 12 choose 4 possible positions of FR,FL,BL,BR edges
        internal const short N_SLICE2 = 24; // 4! permutations of FR,FL,BL,BR edges in phase2
        internal const short N_PARITY = 2; // 2 possible corner parities
        internal const short N_URFtoDLF = 20160; // 8!/(8-6)! permutation of URF,UFL,ULB,UBR,DFR,DLF corners
        internal const short N_FRtoBR = 11880; // 12!/(12-4)! permutation of FR,FL,BL,BR edges
        internal const short N_URtoUL = 1320; // 12!/(12-3)! permutation of UR,UF,UL edges
        internal const short N_UBtoDF = 1320; // 12!/(12-3)! permutation of UB,DR,DF edges
        internal const short N_URtoDF = 20160; // 8!/(8-6)! permutation of UR,UF,UL,UB,DR,DF edges in phase2

        internal const int N_URFtoDLB = 40320; // 8! permutations of the corners
        internal const int N_URtoBR = 479001600; // 8! permutations of the corners

        internal const short N_MOVE = 18;

        // All coordinates are 0 for a solved cube except for UBtoDF, which is 114
        internal short twist;
        internal short flip;
        internal short parity;
        internal short FRtoBR;
        internal short URFtoDLF;
        internal short URtoUL;
        internal short UBtoDF;
        internal int URtoDF;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Generate a CoordCube from a CubieCube
        internal CoordCube(CubieCube c, DateTime startTime, string currentTime, out string info)
        {
            info = currentTime;
            twist = c.getTwist();

            flip = c.getFlip();
            parity = c.cornerParity();
            FRtoBR = c.getFRtoBR();

            URFtoDLF = c.getURFtoDLF();
            URtoUL = c.getURtoUL();
            UBtoDF = c.getUBtoDF();
            URtoDF = c.getURtoDF();// only needed in phase2
            info += "[ Finished Initialiation: " + String.Format(@"{0:mm\:ss\.ffff}", (DateTime.Now - startTime)) + " ] ";

        }

        // A move on the coordinate level
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        internal virtual void move(int m)
        {
            twist = twistMove[twist, m];
            flip = flipMove[flip, m];
            parity = parityMove[parity][m];
            FRtoBR = FRtoBR_Move[FRtoBR, m];
            URFtoDLF = URFtoDLF_Move[URFtoDLF, m];
            URtoUL = URtoUL_Move[URtoUL, m];
            UBtoDF = UBtoDF_Move[UBtoDF, m];
            if (URtoUL < 336 && UBtoDF < 336) // updated only if UR,UF,UL,UB,DR,DF
            {
                // are not in UD-slice
                URtoDF = MergeURtoULandUBtoDF[URtoUL, UBtoDF];
            }
        }


        // ******************************************Phase 1 move tables*****************************************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Move table for the twists of the corners
        // twist < 2187 in phase 2.
        // twist = 0 in phase 2.
        internal static short[,] twistMove = CoordCubeTables.twist;
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Move table for the flips of the edges
        // flip < 2048 in phase 1
        // flip = 0 in phase 2.
        internal static short[,] flipMove = CoordCubeTables.flip;
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Parity of the corner permutation. This is the same as the parity for the edge permutation of a valid cube.
        // parity has values 0 and 1
        internal static short[][] parityMove = new short[][]
        {
        new short[] {1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1},
        new short[] {0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0}
        };


        // ***********************************Phase 1 and 2 movetable********************************************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Move table for the four UD-slice edges FR, FL, Bl and BR
        // FRtoBRMove < 11880 in phase 1
        // FRtoBRMove < 24 in phase 2
        // FRtoBRMove = 0 for solved cube
        internal static short[,] FRtoBR_Move = CoordCubeTables.FRtoBR;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Move table for permutation of six corners. The positions of the DBL and DRB corners are determined by the parity.
        // URFtoDLF < 20160 in phase 1
        // URFtoDLF < 20160 in phase 2
        // URFtoDLF = 0 for solved cube.
        internal static short[,] URFtoDLF_Move = CoordCubeTables.URFtoDLF;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Move table for the permutation of six U-face and D-face edges in phase2. The positions of the DL and DB edges are
        // determined by the parity.
        // URtoDF < 665280 in phase 1
        // URtoDF < 20160 in phase 2
        // URtoDF = 0 for solved cube.
        internal static short[,] URtoDF_Move = CoordCubeTables.URtoDF;

        // **************************helper move tables to compute URtoDF for the beginning of phase2************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Move table for the three edges UR,UF and UL in phase1.
        internal static short[,] URtoUL_Move = CoordCubeTables.URtoUL;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Move table for the three edges UB,DR and DF in phase1.
        internal static short[,] UBtoDF_Move = CoordCubeTables.UBtoDF;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Table to merge the coordinates of the UR,UF,UL and UB,DR,DF edges at the beginning of phase2
        internal static short[,] MergeURtoULandUBtoDF = CoordCubeTables.MergeURtoULandUBtoDF;


        // ****************************************Pruning tables for the search*********************************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Pruning table for the permutation of the corners and the UD-slice edges in phase2.
        // The pruning table entries give a lower estimation for the number of moves to reach the solved cube.
        internal static sbyte[] Slice_URFtoDLF_Parity_Prun = CoordCubeTables.Slice_URFtoDLF_Parity_Prun;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Pruning table for the permutation of the edges in phase2.
        // The pruning table entries give a lower estimation for the number of moves to reach the solved cube.
        internal static sbyte[] Slice_URtoDF_Parity_Prun = CoordCubeTables.Slice_URtoDF_Parity_Prun;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Pruning table for the twist of the corners and the position (not permutation) of the UD-slice edges in phase1
        // The pruning table entries give a lower estimation for the number of moves to reach the H-subgroup.
        internal static sbyte[] Slice_Twist_Prun = CoordCubeTables.Slice_Twist_Prun;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Pruning table for the flip of the edges and the position (not permutation) of the UD-slice edges in phase1
        // The pruning table entries give a lower estimation for the number of moves to reach the H-subgroup.
        internal static sbyte[] Slice_Flip_Prun = CoordCubeTables.Slice_Flip_Prun;
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

 
        // Set pruning value in table. Two values are stored in one byte.
        internal static void setPruning(sbyte[] table, int index, sbyte value)
        {
            if ((index & 1) == 0)
            {
                table[index / 2] &= unchecked((sbyte)(0xf0 | value));
            }
            else
            {
                table[index / 2] &= (sbyte)(0x0f | (value << 4));
            }
        }

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Extract pruning value
        internal static sbyte getPruning(sbyte[] table, int index)
        {
            if ((index & 1) == 0)
            {
                return (sbyte)(table[index / 2] & 0x0f);
            }
            else
            {
                return (sbyte)((int)((uint)(table[index / 2] & 0xf0) >> 4));
            }
        }
    }

    public static class CoordCubeTables
    {
        
        // Movement
        public static readonly short[,] twist = Tools.DeserializeTable("twist");
        public static readonly short[,] flip = Tools.DeserializeTable("flip");
        public static readonly short[,] FRtoBR = Tools.DeserializeTable("FRtoBR");
        public static readonly short[,] URFtoDLF = Tools.DeserializeTable("URFtoDLF");
        public static readonly short[,] URtoDF = Tools.DeserializeTable("URtoDF");
        public static readonly short[,] URtoUL = Tools.DeserializeTable("URtoUL");
        public static readonly short[,] UBtoDF = Tools.DeserializeTable("UBtoDF");
        public static readonly short[,] MergeURtoULandUBtoDF = Tools.DeserializeTable("MergeURtoULandUBtoDF");        

        //Prune
        public static readonly sbyte[] Slice_URFtoDLF_Parity_Prun = Tools.DeserializeSbyteArray("Slice_URFtoDLF_Parity_Prun");
        public static readonly sbyte[] Slice_URtoDF_Parity_Prun = Tools.DeserializeSbyteArray("Slice_URtoDF_Parity_Prun");
        public static readonly sbyte[] Slice_Twist_Prun = Tools.DeserializeSbyteArray("Slice_Twist_Prun");
        public static readonly sbyte[] Slice_Flip_Prun = Tools.DeserializeSbyteArray("Slice_Flip_Prun");
        
    }
    

}
