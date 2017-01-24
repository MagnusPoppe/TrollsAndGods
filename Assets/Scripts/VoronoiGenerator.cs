﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using csDelaunay;


public class VoronoiGenerator 
{
    // The number of polygons/sites we want
    int numberOfSites;
    static Color EDGECOLOR = Color.blue;
    static Color POINTCOLOR = Color.red;

    Texture2D tx;

    // This is where we will store the resulting data
    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;

    /// <summary>
    /// Initializes a new instance of the <see cref="VoronoiGenerator"/> class.
    /// </summary>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <param name="pkt">Pkt.</param>
    public VoronoiGenerator(int width, int height, Vector2[] pkt, int relax) 
    {
        numberOfSites = pkt.Length;

        // Create your sites (lets call that the center of your polygons)
        List<Vector2f> points = UsePoints(pkt);

        // Create the bounds of the voronoi diagram
        // Use Rectf instead of Rect; it's a struct just like Rect and does pretty much the same,
        // but like that it allows you to run the delaunay library outside of unity (which mean also in another tread)
        Rectf bounds = new Rectf(0,0,width,height);

        // There is a two ways you can create the voronoi diagram: with or without the lloyd relaxation
        // Here I used it with 2 iterations of the lloyd relaxation
        Voronoi voronoi = new Voronoi(points,bounds, relax);

        // But you could also create it without lloyd relaxtion and call that function later if you want
        //Voronoi voronoi = new Voronoi(points,bounds);
        //voronoi.LloydRelaxation(5);

        // Now retreive the edges from it, and the new sites position if you used lloyd relaxtion
        sites = voronoi.SitesIndexedByLocation;
        edges = voronoi.Edges;

        tx = DrawVoronoiDiagram(width, height);
    }

    /// <summary>
    /// Gets the texture of the diagram.
    /// </summary>
    /// <returns>The texture.</returns>
    public Texture2D getTexture()
    {
        return tx;
    }


    /// <summary>
    /// Takes an array of vector2 points and converts it into 
    /// readable points for the voronoi algorithm.
    /// </summary>
    /// <returns>The points as voronoi points.</returns>
    /// <param name="points">Points.</param>
    private List<Vector2f> UsePoints( Vector2[] points )
    {
        List<Vector2f> output = new List<Vector2f>();
        for (int i = 0; i < points.Length; i++)
        {
            output.Add(new Vector2f(points[i].x, points[i].y));
        }
        return output;
    }


    /// <summary>
    /// Draws the voronoi diagram. Here is a very simple way 
    /// to display the result using a simple bresenham line algorithm.
    /// This algorithm fills out a Texture2D object and returns it.
    /// 
    /// -  Sets POINTCOLOR for points of interest.
    /// -  Sets EDGECOLOR for edges.
    /// -  Sets 0 for empty squares. This is default. No changes.
    /// </summary>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    /// <returns>Texture2D object containing the map.</returns>
    private Texture2D DrawVoronoiDiagram(int width, int height) 
    {
        tx = new Texture2D(width,height);
        foreach (KeyValuePair<Vector2f,Site> kv in sites) 
        {
            tx.SetPixel((int)kv.Key.x, (int)kv.Key.y, POINTCOLOR);
        }
        foreach (Edge edge in edges) 
        {
            // if the edge doesn't have clippedEnds, if was not within the bounds, dont draw it
            if (edge.ClippedEnds == null) 
                continue;
            DrawLine(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], tx, EDGECOLOR);
        }
        tx.Apply();
        return tx;
    }

    // Bresenham line algorithm
    private void DrawLine(Vector2f p0, Vector2f p1, Texture2D tx, Color c, int offset = 0) {
        int x0 = (int)p0.x;
        int y0 = (int)p0.y;
        int x1 = (int)p1.x;
        int y1 = (int)p1.y;

        int dx = Mathf.Abs(x1-x0);
        int dy = Mathf.Abs(y1-y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx-dy;

        while (true) {
            tx.SetPixel(x0+offset,y0+offset,c);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2*err;
            if (e2 > -dy) {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx) {
                err += dx;
                y0 += sy;
            }
        }
    }
}      