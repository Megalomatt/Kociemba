using System;


namespace Kociemba
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Cube on the facelet level
    public class FaceCube
    {
        public CubeColor[] f = new CubeColor[54];

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Map the corner positions to facelet positions. cornerFacelet[URF.ordinal()][0] e.g. gives the position of the
        // facelet in the URF corner position, which defines the orientation.<br>
        // cornerFacelet[URF.ordinal()][1] and cornerFacelet[URF.ordinal()][2] give the position of the other two facelets
        // of the URF corner (clockwise).
        public static Facelet[][] cornerFacelet = new Facelet[][] {
            new Facelet[] { Facelet.U9, Facelet.R1, Facelet.F3 },
            new Facelet[] { Facelet.U7, Facelet.F1, Facelet.L3 },
            new Facelet[] { Facelet.U1, Facelet.L1, Facelet.B3 },
            new Facelet[] { Facelet.U3, Facelet.B1, Facelet.R3 },
            new Facelet[] { Facelet.D3, Facelet.F9, Facelet.R7 },
            new Facelet[] { Facelet.D1, Facelet.L9, Facelet.F7 },
            new Facelet[] { Facelet.D7, Facelet.B9, Facelet.L7 },
            new Facelet[] { Facelet.D9, Facelet.R9, Facelet.B7 }
            };

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Map the edge positions to facelet positions. edgeFacelet[UR.ordinal()][0] e.g. gives the position of the facelet in
        // the UR edge position, which defines the orientation.<br>
        // edgeFacelet[UR.ordinal()][1] gives the position of the other facelet
        public static Facelet[][] edgeFacelet = new Facelet[][]{
            new Facelet[] { Facelet.U6, Facelet.R2 },
            new Facelet[] { Facelet.U8, Facelet.F2 },
            new Facelet[] { Facelet.U4, Facelet.L2 },
            new Facelet[] { Facelet.U2, Facelet.B2 },
            new Facelet[] { Facelet.D6, Facelet.R8 },
            new Facelet[] { Facelet.D2, Facelet.F8 },
            new Facelet[] { Facelet.D4, Facelet.L8 },
            new Facelet[] { Facelet.D8, Facelet.B8 },
            new Facelet[] { Facelet.F6, Facelet.R4 },
            new Facelet[] { Facelet.F4, Facelet.L6 },
            new Facelet[] { Facelet.B6, Facelet.L4 },
            new Facelet[] { Facelet.B4, Facelet.R6 }
        };

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Map the corner positions to facelet CubeColors.
        public static CubeColor[][] cornerColor = new CubeColor[][]{
            new CubeColor[] { CubeColor.U, CubeColor.R, CubeColor.F },
            new CubeColor[] { CubeColor.U, CubeColor.F, CubeColor.L },
            new CubeColor[] { CubeColor.U, CubeColor.L, CubeColor.B },
            new CubeColor[] { CubeColor.U, CubeColor.B, CubeColor.R },
            new CubeColor[] { CubeColor.D, CubeColor.F, CubeColor.R },
            new CubeColor[] { CubeColor.D, CubeColor.L, CubeColor.F },
            new CubeColor[] { CubeColor.D, CubeColor.B, CubeColor.L },
            new CubeColor[] { CubeColor.D, CubeColor.R, CubeColor.B }
        };

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Map the edge positions to facelet CubeColors.
        public static CubeColor[][] edgeColor = new CubeColor[][]{
            new CubeColor[] { CubeColor.U, CubeColor.R },
            new CubeColor[] { CubeColor.U, CubeColor.F },
            new CubeColor[] { CubeColor.U, CubeColor.L },
            new CubeColor[] { CubeColor.U, CubeColor.B },
            new CubeColor[] { CubeColor.D, CubeColor.R },
            new CubeColor[] { CubeColor.D, CubeColor.F },
            new CubeColor[] { CubeColor.D, CubeColor.L },
            new CubeColor[] { CubeColor.D, CubeColor.B },
            new CubeColor[] { CubeColor.F, CubeColor.R },
            new CubeColor[] { CubeColor.F, CubeColor.L },
            new CubeColor[] { CubeColor.B, CubeColor.L },
            new CubeColor[] { CubeColor.B, CubeColor.R }
        };

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public FaceCube()
        {
            string s = "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB";
            for (int i = 0; i < 54; i++)
            {
                CubeColor col = (CubeColor)Enum.Parse(typeof(CubeColor), s[i].ToString());
                f[i] = col;
            }

        }

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Construct a facelet cube from a string
        public FaceCube(string cubeString)
        {
            for (int i = 0; i < cubeString.Length; i++)
            {
                CubeColor col = (CubeColor)Enum.Parse(typeof(CubeColor), cubeString[i].ToString());
                f[i] = col;
            }
  
        }

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Gives string representation of a facelet cube
        public string to_fc_String()
        {
            string s = "";
            for (int i = 0; i < 54; i++)
                s += f[i].ToString();
            return s;
        }

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Gives CubieCube representation of a faceletcube
        public CubieCube toCubieCube()
        {
            byte ori;
            CubieCube ccRet = new CubieCube();
            for (int i = 0; i < 8; i++)
                ccRet.cp[i] = Corner.URF;// invalidate corners
            for (int i = 0; i < 12; i++)
                ccRet.ep[i] = Edge.UR;// and edges
            CubeColor col1, col2;
            foreach (Corner i in (Corner[])Enum.GetValues(typeof(Corner)))
            {
                // get the CubeColors of the cubie at corner i, starting with U/D
                for (ori = 0; ori < 3; ori++)
                    if (f[(int)cornerFacelet[(int)i][ori]] == CubeColor.U || f[(int)cornerFacelet[(int)i][ori]] == CubeColor.D)
                        break;
                col1 = f[(int)cornerFacelet[(int)i][(ori + 1) % 3]];
                col2 = f[(int)cornerFacelet[(int)i][(ori + 2) % 3]];

                foreach (Corner j in (Corner[])Enum.GetValues(typeof(Corner)))
                {
                    if (col1 == cornerColor[(int)j][1] && col2 == cornerColor[(int)j][2])
                    {
                        // in cornerposition i we have cornercubie j
                        ccRet.cp[(int)i] = j;
                        ccRet.co[(int)i] = (byte)(ori % 3);
                        break;
                    }
                }
            }
            foreach (Edge i in (Edge[])Enum.GetValues(typeof(Edge)))
                foreach (Edge j in (Edge[])Enum.GetValues(typeof(Edge)))
                {
                    if (f[(int)edgeFacelet[(int)i][0]] == edgeColor[(int)j][0]
                            && f[(int)edgeFacelet[(int)i][1]] == edgeColor[(int)j][1])
                    {
                        ccRet.ep[(int)i] = j;
                        ccRet.eo[(int)i] = 0;
                        break;
                    }
                    if (f[(int)edgeFacelet[(int)i][0]] == edgeColor[(int)j][1]
                            && f[(int)edgeFacelet[(int)i][1]] == edgeColor[(int)j][0])
                    {
                        ccRet.ep[(int)i] = j;
                        ccRet.eo[(int)i] = 1;
                        break;
                    }
                }
            return ccRet;
        }


        //public Cube toCube()
        //{
        //    byte ori;
        //    Cube ccRet = new Cube();
        //    for (int i = 0; i < 8; i++)
        //        ccRet.cp[i] = Corner.URF;// invalidate corners
        //    for (int i = 0; i < 12; i++)
        //        ccRet.ep[i] = Edge.UR;// and edges
        //    CubeColor col1, col2;
        //    foreach (Corner i in (Corner[])Enum.GetValues(typeof(Corner)))
        //    {
        //        // get the CubeColors of the cubie at corner i, starting with U/D
        //        for (ori = 0; ori < 3; ori++)
        //            if (f[(int)cornerFacelet[(int)i][ori]] == CubeColor.U || f[(int)cornerFacelet[(int)i][ori]] == CubeColor.D)
        //                break;
        //        col1 = f[(int)cornerFacelet[(int)i][(ori + 1) % 3]];
        //        col2 = f[(int)cornerFacelet[(int)i][(ori + 2) % 3]];

        //        foreach (Corner j in (Corner[])Enum.GetValues(typeof(Corner)))
        //        {
        //            if (col1 == cornerColor[(int)j][1] && col2 == cornerColor[(int)j][2])
        //            {
        //                // in cornerposition i we have cornercubie j
        //                ccRet.cp[(int)i] = j;
        //                ccRet.co[(int)i] = (byte)(ori % 3);
        //                break;
        //            }
        //        }
        //    }
        //    foreach (Edge i in (Edge[])Enum.GetValues(typeof(Edge)))
        //        foreach (Edge j in (Edge[])Enum.GetValues(typeof(Edge)))
        //        {
        //            if (f[(int)edgeFacelet[(int)i][0]] == edgeColor[(int)j][0]
        //                    && f[(int)edgeFacelet[(int)i][1]] == edgeColor[(int)j][1])
        //            {
        //                ccRet.ep[(int)i] = j;
        //                ccRet.eo[(int)i] = 0;
        //                break;
        //            }
        //            if (f[(int)edgeFacelet[(int)i][0]] == edgeColor[(int)j][1]
        //                    && f[(int)edgeFacelet[(int)i][1]] == edgeColor[(int)j][0])
        //            {
        //                ccRet.ep[(int)i] = j;
        //                ccRet.eo[(int)i] = 1;
        //                break;
        //            }
        //        }
        //    return ccRet;
        //}
    }

}
