using System;
using System.Collections.Generic;
using System.Text;

namespace GpcNET
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;

    public enum GpcOperation
    {
        Difference = 0,
        Intersection = 1,
        XOr = 2,
        Union = 3
    }

    public struct Vertex
    {
        public double X;
        public double Y;

        public Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "(" + X.ToString() + "," + Y.ToString() + ")";
        }
    }

    public class VertexList
    {
        public int NofVertices;
        public Vertex[] Vertex;

        public VertexList()
        {
        }

        public VertexList(PointF[] p)
        {
            NofVertices = p.Length;
            Vertex = new Vertex[NofVertices];
            for (int i = 0; i < p.Length; i++)
                Vertex[i] = new Vertex((double)p[i].X, (double)p[i].Y);
        }

        public GraphicsPath ToGraphicsPath()
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLines(ToPoints());
            return graphicsPath;
        }

        public PointF[] ToPoints()
        {
            PointF[] vertexArray = new PointF[NofVertices];
            for (int i = 0; i < NofVertices; i++)
            {
                vertexArray[i] = new PointF((float)Vertex[i].X, (float)Vertex[i].Y);
            }
            return vertexArray;
        }

        public GraphicsPath TristripToGraphicsPath()
        {
            GraphicsPath graphicsPath = new GraphicsPath();

            for (int i = 0; i < NofVertices - 2; i++)
            {
                graphicsPath.AddPolygon(new PointF[3]{ new PointF( (float)Vertex[i].X,   (float)Vertex[i].Y ),
				                                        new PointF( (float)Vertex[i+1].X, (float)Vertex[i+1].Y ),
				                                        new PointF( (float)Vertex[i+2].X, (float)Vertex[i+2].Y )  });
            }

            return graphicsPath;
        }

        public override string ToString()
        {
            string s = "Polygon with " + NofVertices + " vertices: ";

            for (int i = 0; i < NofVertices; i++)
            {
                s += Vertex[i].ToString();
                if (i != NofVertices - 1)
                    s += ",";
            }
            return s;
        }
    }

    public class Polygon
    {
        public int NofContours;
        public bool[] ContourIsHole;
        public VertexList[] Contour;

        public Polygon()
        {
        }

        // path should contain only polylines ( use Flatten )
        // furthermore the constructor assumes that all Subpathes of path except the first one are holes
        public Polygon(GraphicsPath path)
        {
            NofContours = 0;
            foreach (byte b in path.PathTypes)
            {
                if ((b & ((byte)PathPointType.CloseSubpath)) != 0)
                    NofContours++;
            }

            ContourIsHole = new bool[NofContours];
            Contour = new VertexList[NofContours];
            for (int i = 0; i < NofContours; i++)
                ContourIsHole[i] = (i == 0);

            int contourNr = 0;
            ArrayList contour = new ArrayList();
            for (int i = 0; i < path.PathPoints.Length; i++)
            {
                contour.Add(path.PathPoints[i]);
                if ((path.PathTypes[i] & ((byte)PathPointType.CloseSubpath)) != 0)
                {
                    PointF[] pointArray = (PointF[])contour.ToArray(typeof(PointF));
                    VertexList vl = new VertexList(pointArray);
                    Contour[contourNr++] = vl;
                    contour.Clear();
                }
            }
        }

        public static Polygon FromFile(string filename, bool readHoleFlags)
        {
            return GpcWrapper.ReadPolygon(filename, readHoleFlags);
        }

        public void AddContour(VertexList contour, bool contourIsHole)
        {
            bool[] hole = new bool[NofContours + 1];
            VertexList[] cont = new VertexList[NofContours + 1];

            for (int i = 0; i < NofContours; i++)
            {
                hole[i] = ContourIsHole[i];
                cont[i] = Contour[i];
            }
            hole[NofContours] = contourIsHole;
            cont[NofContours++] = contour;

            ContourIsHole = hole;
            Contour = cont;
        }

        public GraphicsPath ToGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();

            for (int i = 0; i < NofContours; i++)
            {
                PointF[] points = Contour[i].ToPoints();
                if (ContourIsHole[i])
                    Array.Reverse(points);
                path.AddPolygon(points);
            }
            return path;
        }

        public override string ToString()
        {
            string s = "Polygon with " + NofContours.ToString() + " contours." + "\r\n";
            for (int i = 0; i < NofContours; i++)
            {
                if (ContourIsHole[i])
                    s += "Hole: ";
                else
                    s += "Contour: ";
                s += Contour[i].ToString();
            }
            return s;
        }

        public Tristrip ClipToTristrip(GpcOperation operation, Polygon polygon)
        {
            return GpcWrapper.ClipToTristrip(operation, this, polygon);
        }

        public Polygon Clip(GpcOperation operation, Polygon polygon)
        {
            return GpcWrapper.Clip(operation, this, polygon);
        }

        public Tristrip ToTristrip()
        {
            return GpcWrapper.PolygonToTristrip(this);
        }

        public void Save(string filename, bool writeHoleFlags)
        {
            GpcWrapper.SavePolygon(filename, writeHoleFlags, this);
        }
    }

    public class Tristrip
    {
        public int NofStrips;
        public VertexList[] Strip;
    }

    public class GpcWrapper
    {
        public static Tristrip PolygonToTristrip(Polygon polygon)
        {
            gpc_tristrip gpc_strip = new gpc_tristrip();
            gpc_polygon gpc_pol = GpcWrapper.PolygonTo_gpc_polygon(polygon);
            gpc_polygon_to_tristrip(ref gpc_pol, ref gpc_strip);
            Tristrip tristrip = GpcWrapper.gpc_strip_ToTristrip(gpc_strip);

            GpcWrapper.Free_gpc_polygon(gpc_pol);
            GpcWrapper.gpc_free_tristrip(ref gpc_strip);

            return tristrip;
        }

        public static Tristrip ClipToTristrip(GpcOperation operation, Polygon subject_polygon, Polygon clip_polygon)
        {
            gpc_tristrip gpc_strip = new gpc_tristrip();
            gpc_polygon gpc_subject_polygon = GpcWrapper.PolygonTo_gpc_polygon(subject_polygon);
            gpc_polygon gpc_clip_polygon = GpcWrapper.PolygonTo_gpc_polygon(clip_polygon);

            gpc_tristrip_clip(operation, ref gpc_subject_polygon, ref gpc_clip_polygon, ref gpc_strip);
            Tristrip tristrip = GpcWrapper.gpc_strip_ToTristrip(gpc_strip);

            GpcWrapper.Free_gpc_polygon(gpc_subject_polygon);
            GpcWrapper.Free_gpc_polygon(gpc_clip_polygon);
            GpcWrapper.gpc_free_tristrip(ref gpc_strip);

            return tristrip;
        }

        public static Polygon Clip(GpcOperation operation, Polygon subject_polygon, Polygon clip_polygon)
        {
            gpc_polygon gpc_polygon = new gpc_polygon();
            gpc_polygon gpc_subject_polygon = GpcWrapper.PolygonTo_gpc_polygon(subject_polygon);
            gpc_polygon gpc_clip_polygon = GpcWrapper.PolygonTo_gpc_polygon(clip_polygon);

            gpc_polygon_clip(operation, ref gpc_subject_polygon, ref gpc_clip_polygon, ref gpc_polygon);
            Polygon polygon = GpcWrapper.gpc_polygon_ToPolygon(gpc_polygon);

            GpcWrapper.Free_gpc_polygon(gpc_subject_polygon);
            GpcWrapper.Free_gpc_polygon(gpc_clip_polygon);
            GpcWrapper.gpc_free_polygon(ref gpc_polygon);

            return polygon;
        }

        public static void SavePolygon(string filename, bool writeHoleFlags, Polygon polygon)
        {
            gpc_polygon gpc_polygon = GpcWrapper.PolygonTo_gpc_polygon(polygon);

            IntPtr fp = fopen(filename, "wb");
            gpc_write_polygon(fp, writeHoleFlags ? ((int)1) : ((int)0), ref gpc_polygon);
            fclose(fp);

            GpcWrapper.Free_gpc_polygon(gpc_polygon);
        }

        public static Polygon ReadPolygon(string filename, bool readHoleFlags)
        {
            gpc_polygon gpc_polygon = new gpc_polygon();

            IntPtr fp = fopen(filename, "rb");
            gpc_read_polygon(fp, readHoleFlags ? ((int)1) : ((int)0), ref gpc_polygon);
            Polygon polygon = gpc_polygon_ToPolygon(gpc_polygon);
            gpc_free_polygon(ref gpc_polygon);
            fclose(fp);

            return polygon;
        }

        private static gpc_polygon PolygonTo_gpc_polygon(Polygon polygon)
        {
            gpc_polygon gpc_pol = new gpc_polygon();
            gpc_pol.num_contours = polygon.NofContours;

            int[] hole = new int[polygon.NofContours];
            for (int i = 0; i < polygon.NofContours; i++)
                hole[i] = (polygon.ContourIsHole[i] ? 1 : 0);
            gpc_pol.hole = Marshal.AllocCoTaskMem(polygon.NofContours * Marshal.SizeOf(hole[0]));
            Marshal.Copy(hole, 0, gpc_pol.hole, polygon.NofContours);

            gpc_pol.contour = Marshal.AllocCoTaskMem(polygon.NofContours * Marshal.SizeOf(new gpc_vertex_list()));
            IntPtr ptr = gpc_pol.contour;
            for (int i = 0; i < polygon.NofContours; i++)
            {
                gpc_vertex_list gpc_vtx_list = new gpc_vertex_list();
                gpc_vtx_list.num_vertices = polygon.Contour[i].NofVertices;
                gpc_vtx_list.vertex = Marshal.AllocCoTaskMem(polygon.Contour[i].NofVertices * Marshal.SizeOf(new gpc_vertex()));
                IntPtr ptr2 = gpc_vtx_list.vertex;
                for (int j = 0; j < polygon.Contour[i].NofVertices; j++)
                {
                    gpc_vertex gpc_vtx = new gpc_vertex();
                    gpc_vtx.x = polygon.Contour[i].Vertex[j].X;
                    gpc_vtx.y = polygon.Contour[i].Vertex[j].Y;
                    Marshal.StructureToPtr(gpc_vtx, ptr2, false);
                    ptr2 = (IntPtr)(((int)ptr2) + Marshal.SizeOf(gpc_vtx));
                }
                Marshal.StructureToPtr(gpc_vtx_list, ptr, false);
                ptr = (IntPtr)(((int)ptr) + Marshal.SizeOf(gpc_vtx_list));
            }

            return gpc_pol;
        }

        private static Polygon gpc_polygon_ToPolygon(gpc_polygon gpc_polygon)
        {
            Polygon polygon = new Polygon();

            polygon.NofContours = gpc_polygon.num_contours;
            polygon.ContourIsHole = new bool[polygon.NofContours];
            polygon.Contour = new VertexList[polygon.NofContours];
            short[] holeShort = new short[polygon.NofContours];
            IntPtr ptr = gpc_polygon.hole;
            Marshal.Copy(gpc_polygon.hole, holeShort, 0, polygon.NofContours);
            for (int i = 0; i < polygon.NofContours; i++)
                polygon.ContourIsHole[i] = (holeShort[i] != 0);

            ptr = gpc_polygon.contour;
            for (int i = 0; i < polygon.NofContours; i++)
            {
                gpc_vertex_list gpc_vtx_list = (gpc_vertex_list)Marshal.PtrToStructure(ptr, typeof(gpc_vertex_list));
                polygon.Contour[i] = new VertexList();
                polygon.Contour[i].NofVertices = gpc_vtx_list.num_vertices;
                polygon.Contour[i].Vertex = new Vertex[polygon.Contour[i].NofVertices];
                IntPtr ptr2 = gpc_vtx_list.vertex;
                for (int j = 0; j < polygon.Contour[i].NofVertices; j++)
                {
                    gpc_vertex gpc_vtx = (gpc_vertex)Marshal.PtrToStructure(ptr2, typeof(gpc_vertex));
                    polygon.Contour[i].Vertex[j].X = gpc_vtx.x;
                    polygon.Contour[i].Vertex[j].Y = gpc_vtx.y;

                    ptr2 = (IntPtr)(((int)ptr2) + Marshal.SizeOf(gpc_vtx));
                }
                ptr = (IntPtr)(((int)ptr) + Marshal.SizeOf(gpc_vtx_list));
            }

            return polygon;
        }

        private static Tristrip gpc_strip_ToTristrip(gpc_tristrip gpc_strip)
        {
            Tristrip tristrip = new Tristrip();
            tristrip.NofStrips = gpc_strip.num_strips;
            tristrip.Strip = new VertexList[tristrip.NofStrips];
            IntPtr ptr = gpc_strip.strip;
            for (int i = 0; i < tristrip.NofStrips; i++)
            {
                tristrip.Strip[i] = new VertexList();
                gpc_vertex_list gpc_vtx_list = (gpc_vertex_list)Marshal.PtrToStructure(ptr, typeof(gpc_vertex_list));
                tristrip.Strip[i].NofVertices = gpc_vtx_list.num_vertices;
                tristrip.Strip[i].Vertex = new Vertex[tristrip.Strip[i].NofVertices];

                IntPtr ptr2 = gpc_vtx_list.vertex;
                for (int j = 0; j < tristrip.Strip[i].NofVertices; j++)
                {
                    gpc_vertex gpc_vtx = (gpc_vertex)Marshal.PtrToStructure(ptr2, typeof(gpc_vertex));
                    tristrip.Strip[i].Vertex[j].X = gpc_vtx.x;
                    tristrip.Strip[i].Vertex[j].Y = gpc_vtx.y;

                    ptr2 = (IntPtr)(((int)ptr2) + Marshal.SizeOf(gpc_vtx));
                }
                ptr = (IntPtr)(((int)ptr) + Marshal.SizeOf(gpc_vtx_list));
            }

            return tristrip;
        }

        private static void Free_gpc_polygon(gpc_polygon gpc_pol)
        {
            Marshal.FreeCoTaskMem(gpc_pol.hole);
            IntPtr ptr = gpc_pol.contour;
            for (int i = 0; i < gpc_pol.num_contours; i++)
            {
                gpc_vertex_list gpc_vtx_list = (gpc_vertex_list)Marshal.PtrToStructure(ptr, typeof(gpc_vertex_list));
                Marshal.FreeCoTaskMem(gpc_vtx_list.vertex);
                ptr = (IntPtr)(((int)ptr) + Marshal.SizeOf(gpc_vtx_list));
            }
        }



        [DllImport("gpc.dll")]
        private static extern void gpc_polygon_to_tristrip([In]     ref gpc_polygon polygon,
                                                           [In, Out] ref gpc_tristrip tristrip);

        [DllImport("gpc.dll")]
        private static extern void gpc_polygon_clip([In]     GpcOperation set_operation,
                                                    [In]     ref gpc_polygon subject_polygon,
                                                    [In]     ref gpc_polygon clip_polygon,
                                                    [In, Out] ref gpc_polygon result_polygon);

        [DllImport("gpc.dll")]
        private static extern void gpc_tristrip_clip([In]     GpcOperation set_operation,
                                                     [In]     ref gpc_polygon subject_polygon,
                                                     [In]     ref gpc_polygon clip_polygon,
                                                     [In, Out] ref gpc_tristrip result_tristrip);

        [DllImport("gpc.dll")]
        private static extern void gpc_free_tristrip([In] ref gpc_tristrip tristrip);

        [DllImport("gpc.dll")]
        private static extern void gpc_free_polygon([In] ref gpc_polygon polygon);

        [DllImport("gpc.dll")]
        private static extern void gpc_read_polygon([In] IntPtr fp, [In] int read_hole_flags, [In, Out] ref gpc_polygon polygon);

        [DllImport("gpc.dll")]
        private static extern void gpc_write_polygon([In] IntPtr fp, [In] int write_hole_flags, [In] ref gpc_polygon polygon);

        [DllImport("msvcr71.dll")]
        private static extern IntPtr fopen([In] string filename, [In] string mode);

        [DllImport("msvcr71.dll")]
        private static extern void fclose([In] IntPtr fp);

        [DllImport("msvcr71.dll")]
        private static extern int fputc([In] int c, [In] IntPtr fp);

        enum gpc_op                                   /* Set operation type                */
        {
            GPC_DIFF = 0,                             /* Difference                        */
            GPC_INT = 1,                             /* Intersection                      */
            GPC_XOR = 2,                             /* Exclusive or                      */
            GPC_UNION = 3                              /* Union                             */
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct gpc_vertex                    /* Polygon vertex structure          */
        {
            public double x;            /* Vertex x component                */
            public double y;            /* vertex y component                */
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct gpc_vertex_list               /* Vertex list structure             */
        {
            public int num_vertices; /* Number of vertices in list        */
            public IntPtr vertex;       /* Vertex array pointer              */
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct gpc_polygon                   /* Polygon set structure             */
        {
            public int num_contours; /* Number of contours in polygon     */
            public IntPtr hole;         /* Hole / external contour flags     */
            public IntPtr contour;      /* Contour array pointer             */
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct gpc_tristrip                  /* Tristrip set structure            */
        {
            public int num_strips;   /* Number of tristrips               */
            public IntPtr strip;        /* Tristrip array pointer            */
        }
    }
}
