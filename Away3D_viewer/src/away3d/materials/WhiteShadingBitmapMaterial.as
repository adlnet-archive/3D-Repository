package away3d.materials
{
	import away3d.arcane;	import away3d.containers.*;
	import away3d.core.base.*;
	import away3d.core.draw.*;
	import away3d.core.light.*;
	import away3d.core.utils.*;
	
	import flash.display.*;
	import flash.filters.*;
	import flash.geom.*;
	import flash.utils.*;

    use namespace arcane;
    
    /**
    * Bitmap material with flat white lighting
    */
    public class WhiteShadingBitmapMaterial extends BitmapMaterial
    {
    	/** @private */
        arcane  override function updateMaterial(source:Object3D, view:View3D):void
        {
        	var _source_lightarray_directionals:Array = source.lightarray.directionals;
        	for each (var directional:DirectionalLight in _source_lightarray_directionals) {
        		if (!directional.diffuseTransform[source] || view.scene.updatedObjects[source]) {
        			directional.setDiffuseTransform(source);
        			_materialDirty = true;
        		}
        		
        		if (!directional.specularTransform[source])
        			directional.specularTransform[source] = new Dictionary(true);
        		
        		if (!directional.specularTransform[source][view] || view.scene.updatedObjects[source] || view.updated) {
        			directional.setSpecularTransform(source, view);
        			_materialDirty = true;
        		}
        	}
        	
        	var source_lightarray_points:Array = source.lightarray.points;
        	for each (var point:PointLight in source_lightarray_points) {
        		if (!point.viewPositions[view] || view.scene.updatedObjects[source] || view.updated) {
        			point.setViewPosition(view);
        			_materialDirty = true;
        		}
        	}
        	
        	super.updateMaterial(source, view);
        }
    	/** @private */
        arcane override function renderTriangle(tri:DrawTriangle):void
        {
        	var shade:FaceNormalShaderVO = shader.getTriangleShade(tri, shininess);
            br = (shade.kar + shade.kag + shade.kab + shade.kdr + shade.kdg + shade.kdb + shade.ksr + shade.ksg + shade.ksb)/3;
			
            _view = tri.view;
			_session = tri.source.session;
			
			if ((br < 1) && (blackrender || ((step < 16) && (!_bitmap.transparent))))
            {
            	_session.renderTriangleBitmap(bitmap, getUVData(tri), tri.screenVertices, tri.screenIndices, tri.startIndex, tri.endIndex, smooth, repeat);
                _session.renderTriangleColor(0x000000, 1 - br, tri.screenVertices, tri.screenCommands, tri.screenIndices, tri.startIndex, tri.endIndex);
            }
            else
            if ((br > 1) && (whiterender))
            {
            	_session.renderTriangleBitmap(bitmap, getUVData(tri), tri.screenVertices, tri.screenIndices, tri.startIndex, tri.endIndex, smooth, repeat);
                _session.renderTriangleColor(0xFFFFFF, (br - 1)*whitek, tri.screenVertices, tri.screenCommands, tri.screenIndices, tri.startIndex, tri.endIndex);
            }
            else
            {
                if (step < 64)
                    if (Math.random() < 0.01)
                        doubleStepTo(64);
                var brightness:Number = ladder(br);
                var bitmap:BitmapData = cache[brightness];
                if (bitmap == null)
                {
                	bitmap = new BitmapData(_bitmap.width, _bitmap.height, true, 0x00000000);
                	colorMatrix.matrix = [brightness, 0, 0, 0, 0, 0, brightness, 0, 0, 0, 0, 0, brightness, 0, 0, 0, 0, 0, 1, 0];
                	bitmap.applyFilter(_bitmap, bitmap.rect, bitmapPoint, colorMatrix);
                    cache[brightness] = bitmap;
                }
                _session.renderTriangleBitmap(bitmap, getUVData(tri), tri.screenVertices, tri.screenIndices, tri.startIndex, tri.endIndex, smooth, repeat);
            }
        }
        
        private var blackrender:Boolean;
        private var whiterender:Boolean;
        private var whitek:Number = 0.2;
		private var bitmapPoint:Point = new Point(0, 0);
		private var colorMatrix:ColorMatrixFilter = new ColorMatrixFilter();
        private var cache:Dictionary;
        private var step:int = 1;
		private var br:Number;
		private var shader:FaceNormalShader = new FaceNormalShader();
		
        private function doubleStepTo(limit:int):void
        {
            if (step < limit)
                step *= 2;
        }
         
        private function ladder(v:Number):Number
        {
            if (v < 1/0xFF)
                return 0;
            if (v > 0xFF)
                v = 0xFF;
            return Math.exp(Math.round(Math.log(v)*step)/step);
        }
    	
        protected override function invalidateFaces(source:Object3D = null, view:View3D = null):void
        {
        	super.invalidateFaces(source, view);
        	
        	CacheStore.whiteShadingCache[_bitmap] = new Dictionary(true);
        }
        
        /**
        * Coefficient for shininess level
        */
        public var shininess:Number;
        
		/**
		 * Creates a new <code>WhiteShadingBitmapMaterial</code> object.
		 * 
		 * @param	bitmap				The bitmapData object to be used as the material's texture.
		 * @param	init	[optional]	An initialisation object for specifying default instance properties.
		 */
        public function WhiteShadingBitmapMaterial(bitmap:BitmapData, init:Object = null)
        {
            super(bitmap, init);
            
            if (!CacheStore.whiteShadingCache[_bitmap])
            	CacheStore.whiteShadingCache[_bitmap] = new Dictionary(true);
            	
            cache = CacheStore.whiteShadingCache[_bitmap];
            
            shininess = ini.getNumber("shininess", 20);
        }
    }
}
