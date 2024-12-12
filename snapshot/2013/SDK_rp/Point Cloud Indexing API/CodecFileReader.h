//
//  (C) Copyright 2009 by Autodesk, Inc.
//
//
//  The information contained herein is confidential, proprietary to Autodesk,
//  Inc., and considered a trade secret as defined in section 499C of the
//  penal code of the State of California.  Use of this information by anyone
//  other than authorized employees of Autodesk, Inc. is granted only under a
//  written non-disclosure agreement, expressly prescribing the scope and
//  manner of such use.
//
//
//         CodecFileReader.h
//
//   Point Cloud Decimation Engine Codec File Reader class definition
//
#if !defined(__CODECFILEREADER_H_)
#define __CODECFILEREADER_H_

#pragma once

class CodecFileReader
{
public:
  virtual ~CodecFileReader() {}

  virtual bool open(const __wchar_t* sourceFile) = 0;
    // returns true if file is opened and is readable.
  virtual void close() = 0;

  virtual void fileStats(double* minPt, double* maxPt, long long& numPoints) = 0;
    // if the file is open, sets
    //   minPt[0] to min x of all points in the file,
    //   minPt[1] to min y of all points in the file,
    //   minPt[2] to min z of all points in the file,
    //   maxPt[0], maxPt[1], maxPt[2] to max x,y,z of all points,
    //   numPoints to total number of points in the file.

  virtual bool readFirstPoint(double& x, double& y, double& z,
                 unsigned char& red, unsigned char& green, unsigned char& blue,
                 unsigned char& classification, float& intensity) = 0;
  virtual bool readNextPoint(double& x, double& y, double& z,
                 unsigned char& red, unsigned char& green, unsigned char& blue,
                 unsigned char& classification, float& intensity) = 0;
    // These two functions sequentially read all the valid points in the file,
    // returning false when there are no more.
    // If point isn't classified, set classification to 0xff.
    // If point doesn't have intensity, set intensity to -1.
};

#endif 