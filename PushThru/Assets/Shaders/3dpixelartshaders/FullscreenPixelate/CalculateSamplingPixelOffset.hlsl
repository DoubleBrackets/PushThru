#ifndef CalculateSamplingPixelOffset_INCLUDED
#define CalculateSamplingPixelOffset_INCLUDED

float AdjustFromRawOffset(float raw_offset, float target_pixel_size)
{
	float halfval = target_pixel_size/2;
	if (raw_offset < halfval)
	{
		return -raw_offset;
	}
	else
	{
		return  target_pixel_size - raw_offset;
	}

}

/*
Used for pixelization effect, to ensure that sub-target pixel movement does not cause pixel swimming
Note: camera still needs to be locked into screen resolution(can only move in one pixel increments)


Takes in offset in world space, world space units per raw resolution pixel

Takes in pixel size, which is the number of resolution pixels for each target (downscaled)pixel
Note: for 30 degree iso camera, this value is double along y axis compared to x
this is what scaled_pixel_size is for
*/

void CalculateSamplingPixelOffset_float(float world_space_offset, float world_space_per_resolution_pixel,float target_pixel_size,float target_pixel_size_scaling,out float sampling_offset)
{
	float pixel_offset = world_space_offset / world_space_per_resolution_pixel;
	pixel_offset = floor(pixel_offset + 0.5);
	float sampling_offset_raw = pixel_offset % (target_pixel_size*target_pixel_size_scaling);
	sampling_offset = AdjustFromRawOffset(sampling_offset_raw/target_pixel_size_scaling,target_pixel_size*target_pixel_size_scaling);
}

#endif