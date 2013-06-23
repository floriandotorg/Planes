using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Planes
{
    struct ProjectionMap
    {
        public Point PixelSize;

        public double Top;
        public double Left;
        public double Bottom;
        public double Right;

        public void setTileMillBounds(double b1, double b2, double b3, double b4)
        {
            Top = b4;
            Left = b1;
            Bottom = b2;
            Right = b3;
        }
    }

    class GeoCoordinate
    {
        private double _lat = 0;
        private double _lon = 0;

        public enum Longitude
        {
            West,
            East
        }

        public enum Latitude
        {
            North,
            South
        }

        public GeoCoordinate()
        { }

        public GeoCoordinate(double lat, double lon)
        {
            _lat = lat;
            _lon = lon;
        }

        public GeoCoordinate(Latitude latitude, double latitudeDegrees, double latitudeMinutes, double latitudeSeconds, Longitude longitude, double longitudeDegrees, double longitudeMinutes, double longitudeSeconds)
        {
            setDSM(latitude, latitudeDegrees, latitudeMinutes, latitudeSeconds, longitude, longitudeDegrees, longitudeMinutes, longitudeSeconds);
        }

        public GeoCoordinate(string dsm)
        {
            setDSM(dsm);
        }

        public void setDSM(Latitude latitude, double latitudeDegrees, double latitudeMinutes, double latitudeSeconds, Longitude longitude, double longitudeDegrees, double longitudeMinutes, double longitudeSeconds)
        {
            _lat = latitudeDegrees + (latitudeMinutes * 60d + latitudeSeconds) / 3600d;

            if (latitude == Latitude.South)
            {
                _lat = -_lat;
            }

            _lon = longitudeDegrees + (longitudeMinutes * 60d + longitudeSeconds) / 3600d;

            if (longitude == Longitude.West)
            {
                _lon = -_lon;
            }
        }

        public void setDSM(string dsm)
        {
            string[] parts = dsm.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Count() != 8)
            {
                throw new ArgumentException();
            }
            
            double latitudeDegrees = Convert.ToDouble(parts[0]);
            double latitudeMinutes = Convert.ToDouble(parts[1]);
            double latitudeSeconds = Convert.ToDouble(parts[2]);
            
            double longitudeDegrees = Convert.ToDouble(parts[4]);
            double longitudeMinutes = Convert.ToDouble(parts[5]);
            double longitudeSeconds = Convert.ToDouble(parts[6]);

            Latitude latitude = Latitude.South;

            if (parts[3] == "N")
            {
                latitude = Latitude.North;
            }

            Longitude longitude = Longitude.East;   
 
            if (parts[7] == "W")
            {
                longitude = Longitude.West;
            }

            setDSM(latitude, latitudeDegrees, latitudeMinutes, latitudeSeconds, longitude, longitudeDegrees, longitudeMinutes, longitudeSeconds);
        }

        public Vector2 PixelPoint(ProjectionMap projectionMap)
        {
            double lon = _lon;
            double lat = _lat;

            double mapWidth = projectionMap.PixelSize.X;
            double mapHeight = projectionMap.PixelSize.Y;

            double mapLonLeft =  projectionMap.Left;
            double mapLonRight = projectionMap.Right;
            double mapLonDelta = mapLonRight - mapLonLeft;

            double mapLatBottom = projectionMap.Bottom;
            double mapLatBottomDegree = mapLatBottom * Math.PI / 180;

            double x = (lon - mapLonLeft) * (mapWidth / mapLonDelta);

            lat *=  Math.PI / 180;
            double worldMapWidth = ((mapWidth / mapLonDelta) * 360) / (2 * Math.PI);
            double mapOffsetY = (worldMapWidth / 2 * Math.Log((1 + Math.Sin(mapLatBottomDegree)) / (1 - Math.Sin(mapLatBottomDegree))));
            double y = mapHeight - ((worldMapWidth / 2 * Math.Log((1 + Math.Sin(lat)) / (1 - Math.Sin(lat)))) - mapOffsetY);

            return new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));
        }

        public GeoCoordinate(Vector2 pixelPoint, ProjectionMap projectionMap)
        {
            double x = pixelPoint.X;
            double y = pixelPoint.Y;

            double mapWidth = projectionMap.PixelSize.X;
            double mapHeight = projectionMap.PixelSize.Y;

            double mapLonLeft = projectionMap.Left;
            double mapLonRight = projectionMap.Right;
            double mapLonDelta = mapLonRight - mapLonLeft;

            double mapLatBottom = projectionMap.Bottom;
            double mapLatBottomDegree = mapLatBottom * Math.PI / 180;

            double worldMapRadius = mapWidth / mapLonDelta * 360/(2 * Math.PI);
            double mapOffsetY = (worldMapRadius / 2 * Math.Log((1 + Math.Sin(mapLatBottomDegree)) / (1 - Math.Sin(mapLatBottomDegree))));
            double equatorY = mapHeight + mapOffsetY;   
            double a = (equatorY-y)/worldMapRadius;

            _lat = 180/Math.PI * (2 * Math.Atan(Math.Exp(a)) - Math.PI/2);
            _lon = mapLonLeft+x/mapWidth*mapLonDelta;
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public double Distance(GeoCoordinate other)
        {
            double R = 6371; // km
            double dLat = DegreeToRadian(other._lat - _lat);
            double dLon = DegreeToRadian(other._lon - _lon);
            double lat1 = DegreeToRadian(_lat);
            double lat2 = DegreeToRadian(other._lat);

            double a = Math.Sin(dLat/2) * Math.Sin(dLat/2) + Math.Sin(dLon/2) * Math.Sin(dLon/2) * Math.Cos(lat1) * Math.Cos(lat2); 
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a)); 

            return R * c;
        }

        public override string ToString()
        {
            return "{" + _lat + " / " + _lon + "}";
        }
    }
}
