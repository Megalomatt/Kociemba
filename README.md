# C# Kociemba Two-Phase Solver

This is a C# port of the [Two-Phase-Algorithm Java package](http://kociemba.org/cube.htm) written by Herbert Kociemba.

All credit for this solution goes to him.

This port was created to allow the code to work with the Unity Game Engine.

Using pre-built tables, can solve cubes in under 22 moves in under 0.01 seconds.

I have made a couple of small modifications (to store the built tables and pass out some info) but, for the most part, all comments remain intact and this package can be used with the original documentation.

An [alternative C# implementation](https://github.com/tremwil/TwoPhaseSolver) exists that accepts a move list, instead of a searchString, as an input by GitHub user Tremwill. 

# Using the package

** Note: If using with Unity, switch to the Unity branch! This will allow tables to be used in WebGL **

## Define the scope

    using Kociemba;

## Preparing the search string

Diagam of cube map from Kociemba's original documention:

                 |************|
                 |*U1**U2**U3*|
                 |************|
                 |*U4**U5**U6*|
                 |************|
                 |*U7**U8**U9*|
     ************|************|************|************|
     *L1**L2**L3*|*F1**F2**F3*|*R1**R2**F3*|*B1**B2**B3*|
     ************|************|************|************|
     *L4**L5**L6*|*F4**F5**F6*|*R4**R5**R6*|*B4**B5**B6*|
     ************|************|************|************|
     *L7**L8**L9*|*F7**F8**F9*|*R7**R8**R9*|*B7**B8**B9*|
     ************|************|************|************|
                 |*D1**D2**D3*|
                 |************|
                 |*D4**D5**D6*|
                 |************|
                 |*D7**D8**D9*|
                 |************|


The searchString need to be ordered by sides Up Right Front Down Left Back with the order of the characters in the string, matching the order as outlined in the diagram.
For example, a solved searchString would be:

    string searchString= "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB";

This searchString has had 90 degree clockwise rotation of the front face applied to it:

    string searchString= "UUUUUULLLURRURRURRFFFFFFFFFRRRDDDDDDLLDLLDLLDBBBBBBBBB";



## SearchRunTime
(The slow one)

Initial search takes a while (over 30 seconds on a gaming PC) as the search tables are built at runtime. After the tables are loaded into memory, subsequent searches take milliseconds. 

    public static string solution(
        string facelets,
        out string info,
        int maxDepth= 22,
        long timeOut = 6000,
        bool useSeparator = false,
        bool buildTables = false
        )
Example use:

This method accepts a string of a scrambled cube as an input and outputs a string of the moves that would solve the scramble string.
It requires an additional string to pass out information.

If buildTables is set to true, it will generate the tables necessary for using the alternative Search class.

The following file tree structure will be generated if it does not exist already:
Assets/Kociemba/Tables/


    string info = "";
    string solution = SearchRunTime.solution(searchString, out info, buildTables: true);
    
    

## Search

(The fast one)

A second or two to read the tables into memory then solves take milliseconds.
This method requires tables to exist. They can be built by the alternative SearchRunTime class.

    public static string solution(
        string facelets,
        out string info,
        int maxDepth = 22,
        long timeOut = 6000,
        bool useSeparator = false
        )

Example use:

This accepts a string of a scrambled cube as an input and outputs a string of the moves that would solve the scramble string.
It requires an additional string to pass out information.

If buildTables is set to true, it will generate the tables necessary for using the alternative Search class.

    string info = "";
    string solution = Search.solution(searchString, out info);


# Notes

Setting the maxDepth int lower than 20 can prevent solves being found and decreasing from 22 can cause the searches to take longer. If increased they become faster. 22 or 23 seem like the sweet spot for speed and low move number.
