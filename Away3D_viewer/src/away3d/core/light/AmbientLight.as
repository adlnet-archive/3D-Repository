package away3d.core.light
{
	import flash.display.*;

    /**
    * Ambient light primitive
    */
    public class AmbientLight extends LightPrimitive
    {
        
        /**
        * Updates the bitmapData object used as the lightmap for ambient light shading.
        * 
        * @param	ambient		The coefficient for ambient light intensity.
        */
        public function updateAmbientBitmap(ambient:Number):void
        {
        	this.ambient = ambient;
        	ambientBitmap = new BitmapData(256, 256, false, int(ambient*red << 16) | int(ambient*green << 8) | int(ambient*blue));
        	ambientBitmap.lock();
        }
    }
}