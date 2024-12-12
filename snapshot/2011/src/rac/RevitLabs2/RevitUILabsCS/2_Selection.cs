#region "Copyright"
//
// (C) Copyright 2010 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#endregion

#region "Imports"

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes; // specify this if you want to save typing for attributes. e.g.
using Autodesk.Revit.UI.Selection; 

#endregion

namespace RevitUILabsCS
{
    //' User Selection 
    //' 
    //' Note: This exercise uses Revit Into Labs. 
    //' Modify your project setting to place the dlls from both labs in one place. 
    //' 
    //' cf. Developer Guide, Section 7: Selection (pp 89) 
    //' 

    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UISelection : IExternalCommand
    {

        //' member variables 
        UIApplication m_rvtUIApp;
        UIDocument m_rvtUIDoc;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //' Get the access to the top most objects. (we may not use them all in this specific lab.) 
            m_rvtUIApp = commandData.Application;
            m_rvtUIDoc = m_rvtUIApp.ActiveUIDocument;

            //' (1) pre-selecetd element is under UIDocument.Selection.Elemens. Classic method. 
            //' you can also modify this selection set. 
            //' 
            SelElementSet selSet = m_rvtUIDoc.Selection.Elements;
            ShowSelectionSet(selSet, "Pre-selection: ");

            try
            {
                //' (2.1) pick methods basics. 
                //' there are four types of pick methods: PickObject, PickObjects, PickElementByRectangle, PickPoint. 
                //' Let's quickly try them out. 
                //' 

                PickMethodsBasics();

                //' (2.2) selection object type 
                //' in addition to selecting objects of type Element, the user can pick faces, edges, and point on element. 
                //' 
                PickFaceEdgePoint();

                //' (2.3) selection filter 
                //' if you want additional selection criteria, such as only to pick a wall, you can use selection filter. 
                //' 

                ApplySelectionFilter();
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException err)
            {

                TaskDialog.Show("Revit UI Labs", "You have canceled selection.");
            }
            catch (Exception ex)
            {

                TaskDialog.Show("Revit UI Labs", "Some other exception caught in CancelSelection()");
            }

            //' (2.4) canceling selection 
            //' when the user cancel or press [Esc] key during the selection, OperationCanceledException will be thrown. 
            //' 
            CancelSelection();

            //' (3) apply what we learned to our small house creation 
            //' we put it as a separate command. See at the bottom of the code. 

            return Result.Succeeded;
        }

        //' show the list of element in the given SelElementSet. 
        //' 
        public void ShowSelectionSet(SelElementSet elemSet, string header)
        {

            //' putting in a form of IList 
            IList<Element> elemList = new List<Element>();

            foreach (Element elem in elemSet)
            {
                elemList.Add(elem);
            }

            //' use the helper function. By now, you should be familier with element handling. 
            //' if not, see Revit Intro Lab3. 

            ShowElementList(elemList, header);
        }

        //' Show basic information about the given element. 
        //' 
        public void ShowBasicElementInfo(Element elem)
        {

            //' let's see what kind of element we got. 
            string s = "You picked: \n" ;

            s += ElementToString(elem);

            //' show what we got. 

            TaskDialog.Show("Revit UI Lab", s);
        }

        //' Pick methods sampler. 
        //' quickly try: PickObject, PickObjects, PickElementByRectangle, PickPoint. 
        //' without specifics about objects we want to pick. 
        //' 
        public void PickMethodsBasics()
        {

            //' (1) Pick Object (we have done this already. But just for the sake of completeness.) 
            PickMethod_PickObject();

            //' (2) Pick Objects 
            PickMethod_PickObjects();

            //' (3) Pick Element By Rectangle 
            PickMethod_PickElementByRectangle();

            //' (4) Pick Point 

            PickMethod_PickPoint();
        }

        //' minimum PickObject 
        //' 
        public void PickMethod_PickObject()
        {

            Reference @ref = m_rvtUIDoc.Selection.PickObject(ObjectType.Element, "Select one element");
            Element elem = @ref.Element;

            ShowBasicElementInfo(elem);
        }

        //' minimum PickObjects 
        //' note: when you run this code, you will see "Finish" and "Cancel" buttons in the dialog bar. 
        //' 
        public void PickMethod_PickObjects()
        {

            IList<Reference> refs = m_rvtUIDoc.Selection.PickObjects(ObjectType.Element, "Select multiple elemens");

            //' put it in a List form. 
            IList<Element> elems = new List<Element>();
            foreach (Reference @ref in refs)
            {
                elems.Add(@ref.Element);
            }
            //' show it. 

            ShowElementList(elems, "Pick Objects: ");
        }

        //' minimum PickElementByRectangle 
        //' 
        public void PickMethod_PickElementByRectangle()
        {

            //' note: PickElementByRectangle returns the list of element. not reference. 
            IList<Element> elems = m_rvtUIDoc.Selection.PickElementsByRectangle("Select by rectangle");

            //' show it. 

            ShowElementList(elems, "Pick By Rectangle: ");
        }

        //' minimum PickPoint 
        //' 
        public void PickMethod_PickPoint()
        {

            XYZ pt = m_rvtUIDoc.Selection.PickPoint("Pick a point");

            //' show it. 
            string msg = "Pick Point: ";
            msg += PointToString(pt);

            TaskDialog.Show("Revit UI Labs", msg);
        }

        //' pick face, edge, point on an element 
        //' objectType options is applicable to PickObject() and PickObjects() 
        //' 
        public void PickFaceEdgePoint()
        {

            //' (1) Face 
            PickFace();

            //' (2) Edge 
            PickEdge();

            //' (3) Point 

            PickPointOnElement();
        }

        public void PickFace()
        {

            Reference refFace = m_rvtUIDoc.Selection.PickObject(ObjectType.Face, "Select a face");
            Face oFace = refFace.GeometryObject as Face;

            //' show a message to the user. 
            string msg = "";
            if (oFace != null)
            {
                msg = "You picked the face of element " + refFace.Element.Id.ToString() + "\n";
            }
            else
            {
                msg = "no Face picked \n";
            }

            TaskDialog.Show("Revit UI Labs", msg);
        }

        public void PickEdge()
        {

            Reference refEdge = m_rvtUIDoc.Selection.PickObject(ObjectType.Edge, "Select an edge");
            Edge oEdge = refEdge.GeometryObject as Edge;

            //' show it. 
            string msg = "";
            if (oEdge != null)
            {
                msg = "You picked an edge of element " + refEdge.Element.Id.ToString() + "\n";
            }
            else
            {
                msg = "no Edge picked \n" ;
            }

            TaskDialog.Show("Revit UI Labs", msg);
        }

        public void PickPointOnElement()
        {

            Reference refPoint = m_rvtUIDoc.Selection.PickObject(ObjectType.PointOnElement, "Select a point on element");
            XYZ pt = refPoint.GlobalPoint;

            //' show it. 
            string msg = "";
            if (pt != null)
            {
                msg = "You picked the point " + PointToString(pt) + " on an element " + refPoint.Element.Id.ToString() + "\n";
            }
            else
            {
                msg = "no Point picked \n";
            }

            TaskDialog.Show("Revit UI Labs", msg);
        }

        //' pick with selection filter 
        //' let's assume we only want to pick up a wall. 
        //' 
        public void ApplySelectionFilter()
        {

            //' pick only a wall 
            PickWall();

            //' pick only a planar face. 

            PickPlanarFace();
        }

        //' selection with wall filter. 
        //' See the bottom of the page to see the selection filter implementation. 
        //' 
        public void PickWall()
        {

            SelectionFilterWall selFilterWall = new SelectionFilterWall();
            Reference @ref = m_rvtUIDoc.Selection.PickObject(ObjectType.Element, selFilterWall, "Select a wall");

            //' show it 
            Element elem = @ref.Element;

            ShowBasicElementInfo(elem);
        }

        //' selection with planar face. 
        //' See the bottom of the page to see the selection filter implementation. 
        //' 
        public void PickPlanarFace()
        {

            //' to call ISelectionFilter.AllowReference, use this. 
            //' this will limit picked face to be planar. 
            SelectionFilterPlanarFace selFilterPlanarFace = new SelectionFilterPlanarFace();
            Reference @ref = m_rvtUIDoc.Selection.PickObject(ObjectType.Face, selFilterPlanarFace, "Select a planar face");
            Face oFace = @ref.GeometryObject as Face;

            //' show a message to the user. 
            string msg = "";
            if (oFace != null)
            {
                msg = "You picked the face of element " + @ref.Element.Id.ToString() + "\n";
            }
            else
            {
                msg = "no Face picked \n" ;
            }


            TaskDialog.Show("Revit UI Labs", msg);
        }

        //' canceling selection 
        //' when the user presses [Esc] key during the selection, OperationCanceledException will be thrown. 
        //' 
        public void CancelSelection()
        {

            try
            {
                Reference @ref = m_rvtUIDoc.Selection.PickObject(ObjectType.Element, "Select one element, or press [Esc] to cancel");
                Element elem = @ref.Element;

                ShowBasicElementInfo(elem);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException err)
            {

                TaskDialog.Show("Revit UI Labs", "You have canceled selection.");
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit UI Labs", "Some other exception caught in CancelSelection()");

            }
        }


        #region "Helper Function"
        //'==================================================================== 
        //' Helper Functions 
        //'==================================================================== 
        //' 
        //' Helper function to display info from a list of elements passed onto. 
        //' (Same as Revit Intro Lab3.) 
        //' 
        public void ShowElementList(IList<Element> elems, string header)
        {

            string s = header + "(" + elems.Count.ToString() + ")" + "\n\n";
            s = s + " - Class - Category - Name (or Family: Type Name) - Id - " + "\n";
            foreach (Element elem in elems)
            {
                s = s + ElementToString(elem);
            }

            TaskDialog.Show("Revit UI Lab", s);
        }


        //' Helper Funtion: summarize an element information as a line of text, 
        //' which is composed of: class, category, name and id. 
        //' name will be "Family: Type" if a given element is ElementType. 
        //' Intended for quick viewing of list of element, for example. 
        //' (Same as Revit Intro Lab3.) 
        //' 
        public string ElementToString(Element elem)
        {

            if (elem == null)
            {
                return "none";
            }

            string name = "";

            if (elem is ElementType)
            {
                Parameter param = elem.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM);
                if (param != null)
                {
                    name = param.AsString();
                }
            }
            else
            {
                name = elem.Name;
            }


            return elem.GetType().Name + "; " + elem.Category.Name + "; " + name + "; " + elem.Id.IntegerValue.ToString() + "\n";
        }

        //' Helper Function: returns XYZ in a string form. 
        //' (Same as Revit Intro Lab2) 
        //' 
        public static string PointToString(XYZ pt)
        {

            if (pt == null)
            {
                return "";
            }


            return "(" + pt.X.ToString("F2") + ", " + pt.Y.ToString("F2") + ", " + pt.Z.ToString("F2") + ")";
        }

        #endregion

    }

    //' selection filter that limit the type of object being picked as wall. 
    //' 
    class SelectionFilterWall : ISelectionFilter
    {

        public bool AllowElement(Element elem)
        {
            if (elem.Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_Walls))
            {
                return true;
            }

            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {


            return true;
        }

    }

    //' selection filter that limit the reference type to be planar face 
    //' 
    class SelectionFilterPlanarFace : ISelectionFilter
    {

        public bool AllowElement(Element elem)
        {


            return true;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {

            //' example: if you want to allow only Planar Face and do some more checking, add this. 
            if (((reference.GeometryObject) is PlanarFace))
            {
                //' do additional checking here if you want to 
                return true;
            }

            return false;
        }

    }

    //' 
    //' Create House with UI added 
    //' 
    //' ask the user to pick two corner points of walls. 
    //' then ask to choose a wall to add a front door. 
    //' 
    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateHouseUI : IExternalCommand
    {

        //' member variables 
        UIApplication m_rvtUIApp;
        UIDocument m_rvtUIDoc;
        //Dim m_rvtApp As Application 
        Document m_rvtDoc;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //' Get the access to the top most objects. (we may not use them all in this specific lab.) 
            m_rvtUIApp = commandData.Application;
            m_rvtUIDoc = m_rvtUIApp.ActiveUIDocument;
            m_rvtDoc = m_rvtUIDoc.Document;

            CreateHouseInteractive(m_rvtUIDoc);


            return Result.Succeeded;
        }

        //' create a simple house with user interactions. 
        //' the user is asekd to pick two corners of rectangluar footprint of a house. 
        //' then which wall to place a front door. 
        //' 
        public static void CreateHouseInteractive(UIDocument rvtUIDoc)
        {

            //' (1) Walls 
            //' pick two corners to place a house with an orthogonal rectangular footprint 
            XYZ pt1 = rvtUIDoc.Selection.PickPoint("Pick the first corner of walls");
            XYZ pt2 = rvtUIDoc.Selection.PickPoint("Pick the second corner");

            //' simply create four walls with orthogonal rectangular profile from the two points picked. 
            List<Wall> walls = RevitIntroVB.ModelCreation.CreateWalls(rvtUIDoc.Document, pt1, pt2);

            //' (2) Door 
            //' pick a wall to add a front door 
            SelectionFilterWall selFilterWall = new SelectionFilterWall();
            Reference @ref = rvtUIDoc.Selection.PickObject(ObjectType.Element, selFilterWall, "Select a wall to place a front door");
            Wall wallFront = @ref.Element as Wall;

            //' add a door to the selected wall 
            RevitIntroVB.ModelCreation.AddDoor(rvtUIDoc.Document, wallFront);

            //' (3) Windows 
            //' add windows to the rest of the walls. 
            for (int i = 0; i <= 3; i++)
            {
                if (!(walls[i].Id.IntegerValue == wallFront.Id.IntegerValue))
                {
                    RevitIntroVB.ModelCreation.AddWindow(rvtUIDoc.Document, walls[i]);
                }
            }

            //' (4) Roofs 
            //' add a roof over the walls' rectangular profile. 

            RevitIntroVB.ModelCreation.AddRoof(rvtUIDoc.Document, walls);
        }

    } 
}
