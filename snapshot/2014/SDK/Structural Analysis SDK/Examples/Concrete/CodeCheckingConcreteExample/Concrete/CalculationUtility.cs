//
// (C) Copyright 2003-2013 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Linq;

namespace CodeCheckingConcreteExample.Concrete
{
   static class CalculationUtility
   {
      /// <summary>
      /// The smallest positive value of force treated as non-zero value.
      /// </summary>
      private const double toleranceForN = 1.0;
      /// <summary>
      /// The smallest positive value of moment treated as non-zero value.
      /// </summary>
      private const double toleranceForM = 1.0;
      /// <summary>
      /// The smallest positive value of reinforcement treated as non-zero value.
      /// </summary>
      private const double toleranceForAs = 0.5e-7;
      /// <summary>
      /// The tolerance for safety factor. The final coefficient should not deviate from the values 1.0 ​​of more than toleranceForSafetyFactor
      /// </summary>
      private const double toleranceForSafetyFactor = 0.005;
      /// <summary>
      /// The smallest value of reinforcement for the iterative loops.
      /// </summary>
      private const double minimumAs = 2.0 * toleranceForAs;
      /// <summary>
      /// The lowest acceptable safety factor in the iterative loops.
      /// </summary>
      private const double minSafetyFactor = 1.0 - toleranceForSafetyFactor;
      /// <summary>
      /// The biggest acceptable safety factor in the iterative loops.
      /// </summary>
      private const double maxSafetyFactor = 1.0 + toleranceForSafetyFactor;
      /// <summary>
      /// The maximum of steps in the iterative loops.
      /// </summary>
      private const int maximumIterationStep = 100;

      /// <summary>
      /// Checks with internal accuracy if reinforcement area equals zero
      /// </summary>
      /// <param name="totalSteelArea">Tested reinforcement area</param>
      /// <returns>True if reinforcement is below margin</returns>
      static public bool IsZeroReinforcement(double totalSteelArea)
      {
         if (totalSteelArea > toleranceForAs)
            return false;
         else
            return true;
      }

      /// <summary>
      /// Returns value of minimal reinforcement
      /// </summary>
      /// <returns>Minimal reinforcement value</returns>
      static public double MinimumReinforcement()
      {
         return minimumAs;
      }

      /// <summary>
      /// Checks with internal accuracy if force equals zero
      /// </summary>
      /// <param name="n">Tested force value</param>
      /// <returns>True if force is below margin zero</returns>
      static public bool IsZeroN(double n)
      {
         if (Math.Abs(n) > toleranceForN)
            return false;
         else
            return true;
      }

      /// <summary>
      /// Checks with internal accuracy if force is greater than zero
      /// </summary>
      /// <param name="n">Tested force value</param>
      /// <returns>True if force is greater than zero</returns>
      static public bool GtZeroN(double n)
      {
         if (n > Double.Epsilon)
            return true;
         else
            return false;
      }

      /// <summary>
      /// Checks with internal accuracy if force is less than zero
      /// </summary>
      /// <param name="n">Tested force value</param>
      /// <returns>True if force is less than zero</returns>
      static public bool LtZeroN(double n)
      {
         if (n < -Double.Epsilon)
            return true;
         else
            return false;
      }

      /// <summary>
      /// Checks with internal accuracy if moment equals zero
      /// </summary>
      /// <param name="m">Tested moment value</param>
      /// <returns>True if moment is below margin zero</returns>
      static public bool IsZeroM(double m)
      {
         if (Math.Abs(m) > toleranceForM)
            return false;
         else
            return true;
      }

      /// <summary>
      /// Checks with internal accuracy if moment is greater than zero
      /// </summary>
      /// <param name="m">Tested moment value</param>
      /// <returns>True if moment is greater than zero</returns>
      static public bool GtZeroM(double m)
      {
         if (m > Double.Epsilon)
            return true;
         else
            return false;
      }

      /// <summary>
      /// Checks with internal accuracy if moment is less than zero
      /// </summary>
      /// <param name="m">Tested moment value</param>
      /// <returns>True if moment is less than zero</returns>
      static public bool LtZeroM(double m)
      {
         if (m < -Double.Epsilon)
            return true;
         else
            return false;
      }

      /// <summary>
      /// Checks if number of iterations exceeded arbitrary maximum number of iterations
      /// </summary>
      /// <param name="i">Tested number of iterations</param>
      /// <returns>True if number of iterations exceeded maximum</returns>
      static public bool IsIterEnd(int i)
      {
         if (i < maximumIterationStep)
            return false;
         else
            return true;
      }

      /// <summary>
      /// Checks if safety factor is above arbitrary minimum
      /// </summary>
      /// <param name="safetyFactor">Tested safety factor value</param>
      /// <returns>True if safety factor is above minimum</returns>
      static public bool IsSafety(double safetyFactor)
      {
         return (safetyFactor >= minSafetyFactor);
      }

      /// <summary>
      /// Checks whether safety factor is within limits of optimal level
      /// </summary>
      /// <param name="safetyFactor">Tested safety factor value</param>
      /// <returns>True if safety factor on optimal level</returns>
      static public bool IsSafetyOptimal(double safetyFactor)
      {
         return (safetyFactor >= minSafetyFactor) && (safetyFactor <= maxSafetyFactor);
      }

      /// <summary>
      /// Returns the value linear function defined by two points in a given point
      /// </summary>
      /// <param name="x1">x coordinate of the first definition point</param>
      /// <param name="y1">y coordinate of the first definition point</param>
      /// <param name="x2">x coordinate of the second definition point</param>
      /// <param name="y2">y coordinate of the second definition point</param>
      /// <param name="x3">x coordinate for which function value is searched</param>
      /// <returns>Value (y coordinate) of function</returns>
      static public double ValueOfLinearFunction(double x1, double y1, double x2, double y2, double x3)
      {
         double y3 = float.MaxValue;
         if (Math.Abs(x1 - x2) > float.MaxValue)
         {
            y3 = y1 + (x3 - x1) * (y2 - y1) / (x2 - x1);
         }
         else if (Math.Abs(y1 - y2) < float.MaxValue)
         {
            y3 = 0.5 * (y1 + y2);
         }
         return y3;
      }

      /// <summary>
      /// Finds roots of quadratic equation
      /// </summary>
      /// <param name="a">equation first parameter</param>
      /// <param name="b">equation second parameter</param>
      /// <param name="c">equation third parameter</param>
      /// <param name="x1">reference to the first root value</param>
      /// <param name="x2">reference to the second root value</param>
      /// <returns>false if equation has no roots</returns>
      static public bool RootsOfQuadraticEquation(double a, double b, double c, ref double x1, ref double x2)
      {
         bool realRoots = true;
         double delta = b * b - 4.0 * a * c;
         if (delta < 0)
         {
            x1 = x2 = Double.NaN;
            realRoots = false;
         }
         else
         {
            delta = Math.Sqrt(delta);
            x1 = (-b + delta) / (2.0 * a);
            x2 = (-b - delta) / (2.0 * a);
         }
         return realRoots;
      }
      /// <summary>
      /// Function created XML format file if rcuapiNet component thrown the exception. 
      /// File will be created in current user TEMP path. This file will be useful to debug the rcuapiNet.
      /// The rcuapiNETSerializer.dll component is necessary. 
      /// rcuapiNETSerializer.dll and rcuapiNET.dll should be in the same localization. 
      /// </summary>
      /// <param name="exception">The exceprtion from rcuapiNET.dll</param>
      static public void SerializeIRCException(Autodesk.CodeChecking.Concrete.IRCException exception)
      {
         String assemblyName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + "rcuapiNETSerializer.dll";

         if (System.IO.File.Exists(assemblyName))
         {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyName);

            Type serializerType = assembly.GetTypes().First(s => s.FullName == "Autodesk.CodeChecking.ConcreteSerializer.Serializer");
            System.Reflection.MethodInfo serializeMethod = serializerType.GetMethod("Serialize");
            object serializedDoc = serializeMethod.Invoke(null, new object[] { exception });
            string docTitle = System.IO.Path.GetTempPath() + "IRCException" + DateTime.Now.Ticks + "_" + System.Threading.Thread.CurrentThread.ManagedThreadId + ".rcx";
            (serializedDoc as System.Xml.Linq.XDocument).Save(docTitle);

         }

      }
   }
}
