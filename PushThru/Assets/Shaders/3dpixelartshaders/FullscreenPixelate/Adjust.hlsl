#ifndef Adjust_INCLUDED
#define Adjust_INCLUDED

void Adjust_float(float raw_offset, float pixel_size, out float adjusted_offset)
{
	float halfval = pixel_size/2;
	if (raw_offset < halfval)
	{
		adjusted_offset = -raw_offset;
	}
	else
	{
		adjusted_offset = pixel_size - raw_offset;
	}

}

#endif