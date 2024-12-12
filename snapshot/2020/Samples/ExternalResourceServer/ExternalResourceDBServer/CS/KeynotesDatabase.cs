//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ExternalResourceDBServer.CS
{
   /// <summary>
   /// A "fake" keynote database used to demonstrate how Revit keynote data can
   /// be generated without reading from a *.txt file.
   /// </summary>
   static class KeynotesDatabase
   {

      /// <summary>
      /// Indicates what version of keynote data is currently available from the database.
      /// </summary>
      public static String CurrentVersion
      {
         // Assume that the server's keynote data is updated at the beginning of every month.
         get { return System.DateTime.Now.ToString("MM-yyyy"); }
      }


      /// <summary>
      /// Validates the specified database 'key.'
      /// </summary>
      /// <param name="key">A string containing the 'key' to a particular set of keynote data
      /// that is available from this 'database.'</param>
      /// <returns>True, if the specified 'key' corresponds to a valid set of keynote data in the
      /// 'database,' else False.</returns>
      public static bool IsValidDBKey(String key)
      {
         return key == "1" || key == "2" || key == "3" || key == "4";
      }


      /// <summary>
      /// Loads keynote data corresponding to the specified 'key' string into a KeyBasedTreeEntriesLoadContent
      /// object.
      /// </summary>
      /// <param name="key">A string containing the 'key' to a particular set of keynote data
      /// that is available from this 'database.'</param>
      /// <param name="kdrlc">A KeyBasedTreeEntriesLoadContent object to which the keynote data will be
      /// added.</param>
      /// <returns></returns>
      public static void LoadKeynoteEntries(String key, ref KeyBasedTreeEntriesLoadContent kdrlc)
      {
         if (!IsValidDBKey(key))
            throw new ArgumentOutOfRangeException("key", key, "The specified key cannot be found in the database");

         if (kdrlc == null)
            throw new ArgumentNullException("kdrlc");

         switch(key)
         {
            case "1":
               {
                  // German 1
                  kdrlc.AddEntry(new KeynoteEntry("01",    "",   "Dienstleistungen, Produktionen"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01", "01", "Fuhrparkkosten"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01",    "01.01",    "Frachten"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01", "01.01.01", "Eigene Fracht"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02", "01.01.01", "Fremdfracht"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.99", "01.01.01", "Sonstige - Fracht"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.02"	, "01.01",    "Fuhrparkleistungen"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.02.01", "01.01.02", "Eigener Fuhrpark"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.02.02", "01.01.02", "Fremder Fuhrpark"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.02.99", "01.01.02", "Sonstige - Fuhrparkleistung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.03"	, "01.01",    "Kran und Stapler"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.03.01", "01.01.03", "Kranentladung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.03.02", "01.01.03", "Staplerkosten"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.03.99", "01.01.03", "Sonstiger - Kran und Stapler"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02",        "01",       "Handwerkliche Leistungen"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01",     "01.02",    "Allgemeine Bauarbeiten"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01",  "01.02.01", "Dachdeckungsarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.02",  "01.02.01", "Elektroinstallation"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.03",  "01.02.01", "Erdarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.04",  "01.02.01", "Klempnerarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.05",  "01.02.01", "Rohbau"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.06",  "01.02.01", "Sanitär- und Heizungsbauarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.07",  "01.02.01", "Trockenbauarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.08",  "01.02.01", "Verglasungsarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.99",  "01.02.01", "Sonstige - allgem. Bauarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02",     "01.02",    "Belagsarbeiten"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01",  "01.02.02", "Asphaltarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02",  "01.02.02", "Estricharbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.03",  "01.02.02", "Mineralgemischeinbringung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.04",  "01.02.02", "Natursteinarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.99",  "01.02.02", "Sonstige - Belagsarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.03",     "01.02",    "Betonarbeiten"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.03.01",  "01.02.03", "Betoninstandhaltung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.03.02",  "01.02.03", "Betonsanierung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.03.99",  "01.02.03", "Sonstige - Betonarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.04",     "01.02",    "Montage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.04.01",  "01.02.04", "Dachmontage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.04.02",  "01.02.04", "Fenstermontage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.04.03",  "01.02.04", "Montagewand-/Unterdeckenmontage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.04.04",  "01.02.04", "Türenmontage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.04.99",  "01.02.04", "Sonstige - Montage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05",     "01.02",    "Verlegung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.01",  "01.02.05", "Fliesenverlegung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.02",  "01.02.05", "Parkett- und Laminatverlegung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.03",  "01.02.05", "Pflasterarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.04",  "01.02.05", "Plattenverlegung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.05",  "01.02.05", "PVC-Verlegung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.06",  "01.02.05", "Tapezierarbeit"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.07",  "01.02.05", "Teppichverlegung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.08",  "01.02.05", "Wand- und Deckenvertäfelung"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05.99",  "01.02.05", "Sonstige - Verlegung"));
                  break;
               }
            case "2":
               {
                  // German 2
                  kdrlc.AddEntry(new KeynoteEntry("02",             "",         "Schüttgüter"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01",          "02",       "Schüttungen, Asche, Salze"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.01",       "02.01",    "Abraum"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.01.01",    "02.01.01", "Humus"));	
                  kdrlc.AddEntry(new KeynoteEntry("02.01.01.02",    "02.01.01", "Mutterboden"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.01.03",    "02.01.01", "Schotter"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.01.04",    "02.01.01", "Torf"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.01.99",    "02.01.01", "Sonstiger - Abraum"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.02",       "02.01",    "Asche"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.02.01",    "02.01.02", "Steinkohlenflugasche"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.02.99",    "02.01.02", "Sonstige - Asche"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.03",       "02.01",    "Bims"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.03.01",    "02.01.03", "Hüttenbims"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.03.02",    "02.01.03", "Naturbims"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.03.99",    "02.01.03", "Sonstiger - Bims"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.04",       "02.01",    "Kies, Kiessand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.04.01",    "02.01.04", "Kies"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.04.01.A1", "02.01",    "Kies"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.04.02",    "02.01.04", "Kiessand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.04.02.A2", "02.01",    "Kiessand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.04.99",    "02.01.04", "Sonstiger - Kies/Kiessand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.05",       "02.01",    "Mineral und Salz"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.05.01",    "02.01.05", "Mineralstoffgemisch"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.05.02",    "02.01.05", "Salz"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.05.99",    "02.01.05", "Sonstiges - Mineral/Salz"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06",       "02.01",    "Sandsorten"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06.01",    "02.01.06", "Quarzsand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06.01.A3", "02.01",    "Quarzsand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06.02",    "02.01.06", "Sand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06.02.A4", "02.01",    "Sand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06.03",    "02.01.06", "Tennissand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06.03.A5", "02.01",    "Tennissand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.06.99",    "02.01.06", "Sonstige - Sandsorte"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.07",       "02.01",    "Schlacke"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.07.01",    "02.01.07", "Hüttenofenschlacke"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.07.02",    "02.01.07", "Lavaschlacke"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.07.03",    "02.01.07", "Schmelzkammerschlacke"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.07.99",    "02.01.07", "Sonstige - Schlacke"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08",       "02.01",    "Splittsorten"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08.01",    "02.01.08", "Splitt"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08.01.A6", "02.01",    "Splitt"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08.02",    "02.01.08", "Splittsand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08.02.A7", "02.01",    "Splittsand"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08.03",    "02.01.08 ", "Ziegelsplitt"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08.03.A8", "02.01",     "Ziegelsplitt"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.08.99",    "02.01.08",  "Sonstige - Splittsorte"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.50",       "02.01",     "Gewachsener Boden"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.50.01",    "02.01.50",  "Gewachsener Boden"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.50.01.A10","02.01",     "Gewachsener Boden"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.50.01.A7", "02.01",     "Gewachsener Boden"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.51",       "02.01",     "Schotter"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.51.01",    "02.01.51",  "Schotter"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.51.01.A6", "02.01",     "Schotter"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01.51.01.A9", "02.01",     "Schotter"));
                  kdrlc.AddEntry(new KeynoteEntry("02.99",          "02",        "Sonstiges"));
                  break;
               }
            case "3":
               {
                  // French 1
                  kdrlc.AddEntry(new KeynoteEntry("01",              "",             "VRD Assainissement"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01",           "01",           "Amélioration et stabilisation des sols"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01",        "01.01",        "Assainissement des sols"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01",     "01.01.01",     "Drain"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01.A1",  "01.01.01.01",  "Tube de drainage PVC 200"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01.A2",  "01.01.01.01",  "Tube de drainage PVC 150"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01.A3",  "01.01.01.01",  "Tube de drainage PVC 100"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01.A4",  "01.01.01.01",  "Tube de drainage terre-cuite 80"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01.A5",  "01.01.01.01",  "Tube de drainage terre-cuite 100"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01.A6",  "01.01.01.01",  "Tube de drainage PVC annelé 80"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.01.A7",  "01.01.01.01",  "Tube de drainage PVC annelé 100"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02",     "01.01.01",     "Nappe drainante"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A1",  "01.01.01.02",  "Nappe drainante 6"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A2",  "01.01.01.02",  "Nappe drainante 8"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A3",  "01.01.01.02",  "Nappe drainante 10"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A4",  "01.01.01.02",  "Nappe drainante 12"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A5",  "01.01.01.02",  "Nappe drainante 14"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A6",  "01.01.01.02",  "Nappe drainante 18"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A7",  "01.01.01.02",  "Nappe drainante 20"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A8",  "01.01.01.02",  "Nappe drainante 24"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.02.A9",  "01.01.01.02",  "Nappe drainante 28"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03",     "01.01.01",     "Matériau de drainage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A1",  "01.01.01.03",  "Drainage Gravier"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A10", "01.01.01.03",  "Drainage Terre naturelle"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A2",  "01.01.01.03",  "Drainage Gravier de compactage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A3",  "01.01.01.03",  "Drainage Sable de quartz"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A4",  "01.01.01.03",  "Drainage Sable"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A5",  "01.01.01.03",  "Drainage Sable de court de tennis"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A6",  "01.01.01.03",  "Drainage Gravillon"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A7",  "01.01.01.03",  "Drainage Gravillon de compactage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A8",  "01.01.01.03",  "Drainage Grave concassé"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.01.03.A9",  "01.01.01.03",  "Drainage Pierraille"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.02",        "01.01",        "Apport d'autre sol"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.03",        "01.01",        "Traitement chimique des sols"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.04",        "01.01",        "Compactage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.01.05",        "01.01",        "Stabilisation de remblais"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02",           "01",           "Voirie"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01",        "01.02",        "Revêtement de voirie"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01",     "01.02.01",     "Revt de voirie Enrobé"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01.A1",  "01.02.01.01",  "Couche Asphalte"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01.A2",  "01.02.01.01",  "Couche Ciment"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01.A3",  "01.02.01.01",  "Couche Anhydrite"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01.A4",  "01.02.01.01",  "Couche Bitume"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01.A5",  "01.02.01.01",  "Couche Gypse"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01.A6",  "01.02.01.01",  "Couche Résine"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.01.A7",  "01.02.01.01",  "Couche Magnésien"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.02",     "01.02.01",     "Revt de voirie Béton"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.01.03",     "01.02.01",     "Revt de voirie Pavage"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02",        "01.02",        "Bordure et caniveau"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01",     "01.02.02",     "Bordure"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01",  "01.02.02.01",  "Bordure en béton"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A1",  "01.02.02.01", "Bordure normalisée T1"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A10", "01.02.02.01", "Bordure haute 12x30"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A11", "01.02.02.01", "Bordure haute 12x25"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A12", "01.02.02.01", "Bordure arrondie 13,5x22"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A13", "01.02.02.01", "Bordure arrondie 16,5x22"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A15", "01.02.02.01", "Bordure basse 6,5x20"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A16", "01.02.02.01", "Bordure basse 8,5x25"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A17", "01.02.02.01", "Bordure basse 8,5x30"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A18", "01.02.02.01", "Bloc pour gazon 5x20"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A19", "01.02.02.01", "Bloc pour gazon 5x25"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A2",  "01.02.02.01", "Bordure normalisée T2"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A3",  "01.02.02.01", "Bordure normalisée T3"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A4",  "01.02.02.01", "Bordure normalisée T4"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A5",  "01.02.02.01", "Bordure normalisée A1"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A6",  "01.02.02.01", "Bordure normalisée A2"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A7",  "01.02.02.01", "Bordure normalisée P1"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A8",  "01.02.02.01", "Bordure haute 15x30"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.01.A9",  "01.02.02.01", "Bordure haute 15x25"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.02",     "01.02.02.01", "Bordure bateau en béton"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.02.D1",  "01.02.02.01", "Bordure bateau en béton 300x250"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.02.D2",  "01.02.02.01", "Bordure centrale bateau en béton 300x250"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.03",     "01.02.02.01", "Bordure en pierre naturelle"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.03.A1",  "01.02.02.01", "Bordure pierre naturelle 80x250"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.03.A2",  "01.02.02.01", "Bordure pierre naturelle 100x300"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.03.A3",  "01.02.02.01", "Bordure pierre granitique 80x250"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.01.03.A4",  "01.02.02.01", "Bordure pierre granitique 100x300"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02",        "01.02.02",    "Caniveau"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.01",     "01.02.02.02", "Caniveau bloc central"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.01.C1",  "01.02.02.02", "Caniveau central 40x100x12 - CC1"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.01.C2",  "01.02.02.02", "Caniveau central 50x100x14 - CC2"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.01.C3",  "01.02.02.02", "Caniveau central 30x40x9"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.01.C4",  "01.02.02.02", "Caniveau central 40x40x11,5"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.01.C5",  "01.02.02.02", "Caniveau central 50x40x13,5"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.01.C6",  "01.02.02.02", "Caniveau central 30x30x12"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.02",     "01.02.02.02", "Caniveau bloc de bordure"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.02.02.02.D1",  "01.02.02.02", "Caniveau en bordure 300x175x125"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.03",           "01.02",       "Marquage et signalisation"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.04",           "01.02",       "Contrôle d'accès"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.05",           "01.02",       "Eqt de sécurité"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.06",           "01.02",       "Eqt de chantier"));
                  kdrlc.AddEntry(new KeynoteEntry("01.02.07",           "01.02",       "Eqt divers"));
                  break;
               }
            case "4":
               {
                  // French 2
                  kdrlc.AddEntry(new KeynoteEntry("02",          "",          "Aménagement ext"));
                  kdrlc.AddEntry(new KeynoteEntry("02.01",       "02",        "Eclairage d'ext et commande (voir 190402)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02",       "02",        "Mobilier et eqt urbain"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.01",    "02.02",     "Stockage des ordures (voir 24.09.04)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.02",    "02.02",     "Kiosque de jardin"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.03",    "02.02",     "Siège et banc public, table"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.04",    "02.02",     "Jardinière, bac, vasque, ..."));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.05",    "02.02",     "Borne à eau, fontaine"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.06",    "02.02",     "Abris bus"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.07",    "02.02",     "Abris bicyclette et moto, porte bicyclette"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.08",    "02.02",     "Cabine téléphonique exte"));
                  kdrlc.AddEntry(new KeynoteEntry("02.02.09",    "02.02",     "Panneau d'affichage ext"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03",       "02",        "Parc et jardin (eqt), espace de rencontre"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.01",    "02.03",     "Abris jardin"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.02",    "02.03",     "Tonnelle et pergola"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.03",    "02.03",     "Véranda, verrière, serre (voir 1811)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.04",    "02.03",     "Bordure de jardin"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.05",    "02.03",     "Bac à sable"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.06",    "02.03",     "Bassin décoratif"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.07",    "02.03",     "Arrosage de jardin (dispositif et access)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.08",    "02.03",     "Grille et protection d'arbre"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.09",    "02.03",     "Mât de pavillon, pavillon, drapeau"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.10",    "02.03",     "Treillage, support de plante"));
                  kdrlc.AddEntry(new KeynoteEntry("02.03.11",    "02.03",     "Mobilier de jardin"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04",       "02",        "Sport et loisir, espace de jeu (eqt)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.01",    "02.04",     "Jeux aquatiques"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.02",    "02.04",     "Jeux pour enfant"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.03",    "02.04",     "Golf miniature"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.04",    "02.04",     "Piste de skateboard"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.05",    "02.04",     "Court de squash"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.06",    "02.04",     "Piscine (eqt et access),"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.07",    "02.04",     "Sanitaire (appareil) (voir 2004)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.08",    "02.04",     "Court de tennis"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.09",    "02.04",     "Piste de patinoire"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.10",    "02.04",     "Gymnase (eqt sportif et)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.11",    "02.04",     "Borne pour camping"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.12",    "02.04",     "Mur d'escalade"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.13",    "02.04",     "Station de ski (eqt)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.14",    "02.04",     "Tribune et podium"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.15",    "02.04",     "Barbecue fixe"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.16",    "02.04",     "Gradin de stade"));
                  kdrlc.AddEntry(new KeynoteEntry("02.04.17",    "02.04",     "Port de plaisance (eqt)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.05",       "02",        "Structure mobile, légère, provisoire (voir 0702)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.06",       "02",        "Espace vert et plantation"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07",       "02",        "Ouvrage ext (eqt)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.01",    "02.07",     "Clôture et mur d'enceinte, poteau de clôture"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.02",    "02.07",     "Portail, portillon et access"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.03",    "02.07",     "Protection des ouvrages (choc)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.04",    "02.07",     "Ecran antibruit (routier, ferroviaire)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.05",    "02.07",     "Métallerie de sûreté (voir 2302)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.06",    "02.07",     "Garde-corps (voir 1805)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.07",    "02.07",     "Ferronnerie d'art et décorative (voir 1806)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.08",    "02.07",     "Pont, passerelle, appui"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.09",    "02.07",     "Revt de sol et mur pierre, ciment (voir 1608)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.10",    "02.07",     "Boîte aux lettres et access (voir 1809)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.11",    "02.07",     "Câble, corde, grillage, filet (voir 2510)"));
                  kdrlc.AddEntry(new KeynoteEntry("02.07.12",    "02.07",     "Etendoir, poteau d'étendage"));
                  kdrlc.AddEntry(new KeynoteEntry("02.99",       "02",        "Prestations en aménagement ext (voir 2702)"));
                  break;
               }
         }
      }
   }
}
