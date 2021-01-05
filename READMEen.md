# PedicleLength
[Japanese Version](README.md)Vascular pedicle length measurement tool

# Specifications
- Dicom input (uncompressed data, Lossless JPEG compression is not supported)
- Click on a point on the pedicle
- Measure the distance connecting points on the pedicle with a straight line

# PedicleLengthCS project
- C sharp Version

## Improvement request
- 3D output of blood vessel shape (for display in 3D viewer)
- DICOM loading progress display
- Insert points (change order?)
- Display points as a three-dimensional sphere (for 3D output)
- Display the software version in the settings save
## Implemented features
- Open DICOM folder
- DICOM information display (voxel size)
- Cursor position information display (x, y coordinates and values)
- Window setting (WL / WW)
- Click to set points
- Distance calculation
- Point list display
- Delete points
- Display slices of clicked points
- Draw a straight line between points
- Points and straight lines can be resized
- Save / load settings

