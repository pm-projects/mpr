﻿using System;
using System.Collections;
using System.Collections.Generic;

using System.Data;
using System.Diagnostics;
using System.IO;
using System.Timers;


namespace mpr_tst.src
{
    public class gVars
    /// Глобальные переменные приложения.
    /**
     Задают режимы отладки, вывода информации, рабочие каталоги
     и другие системные параметры. 

     \callgraph
    */
    {
        public static string version = "0.62.0";     ///
        public static string name = "mprW32";  ///< name of utility
        public static log lfl = null;    ///<
        public static bool developer = false;   ///<  to have additional ability
        public static bool debug = false;   ///<  to write trace info into log
        public static bool verbose = false;   ///<  to be verbose stdout
        public static bool sound = false;    ///<  to make a sound
        public static string error = "";
        public static string cI = "ru-RU";
        public const uint fgrsSz = (uint)705;
        public const uint tickL = 15;
        public const double interval = 0.015625;
        public const uint ticks = 64;
        //     public static  uint lastSpeed       = (uint)80;
        public static uint start = (uint)15;
        public static uint speed = (uint)15;
        public static uint end = (uint)80;
        public static List<pair> rslt = null;
        public static window mwin = null;

        public static long startedTick = 0;
        public static uint bgCnt = 0, tnCnt = 1;
        public static System.Timers.Timer big; //new System.Timers.Timer(10 * 1000);
        public static System.Timers.Timer tiny;//= new System.Timers.Timer(10 * 1000);

        public static uint[] figures = null;
		
		public const string outFile = "./.mpr.out";
		
    }
}

