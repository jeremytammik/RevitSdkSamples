//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.IO;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ReadonlySharedParameters.CS
{
    class SharedParameterBindingManager
		{
			public String Name { get; set; }
			public ForgeTypeId Type { get; set; }
			public bool UserModifiable  { get; set; }
			public String Description { get; set; }
			public bool Instance { get; set; }
			public Definition Definition { get; set; }
			public BuiltInParameterGroup ParameterGroup { get; set; }
			
			public SharedParameterBindingManager ()
			{
				Name = "Invalid";
            Type = new ForgeTypeId { };
				UserModifiable = true;
				Description = "";
				Instance = true;
				Definition = null;
				ParameterGroup = BuiltInParameterGroup.PG_IDENTITY_DATA;
			}
			
			List<BuiltInCategory> m_categories = new List<BuiltInCategory>();
			
			public ExternalDefinitionCreationOptions GetCreationOptions()
			{
				ExternalDefinitionCreationOptions options = new ExternalDefinitionCreationOptions	(Name, Type);
				options.UserModifiable = UserModifiable;
				options.Description = Description;
				return options;
			}
			
			public void AddCategory(BuiltInCategory category)
			{
				m_categories.Add(category);
			}
			
			private CategorySet GetCategories(Document doc)
			{
				Categories categories = doc.Settings.Categories;
				
				CategorySet categorySet = new CategorySet();
				
				foreach (BuiltInCategory bic in m_categories)
				{
					categorySet.Insert(categories.get_Item(bic));
				}
				
				return categorySet;
			}
			
			public void AddBindings(Document doc)
			{
				Binding binding;
				if (Instance)
				{
					binding = new InstanceBinding(GetCategories(doc));
				}
				else
				{
					binding = new TypeBinding(GetCategories(doc));
				}
				// assumes transaction open
				doc.ParameterBindings.Insert(Definition, binding, ParameterGroup);
			}
			
		}

    class Utils
    {

        
    }
		
}
