﻿package away3d.materials.shaders
{
	import away3d.arcane;
	import away3d.containers.*;
	import away3d.core.base.*;
	import away3d.core.draw.*;
	import away3d.core.light.*;
	import away3d.core.math.*;
	import away3d.core.utils.*;
	import away3d.materials.*;
	
	import flash.display.*;
	import flash.geom.*;
	import flash.utils.*;
	
	use namespace arcane;
	
	/**
	 * Diffuse Dot3 shader class for directional lighting.
	 * 
	 * @see away3d.lights.DirectionalLight3D
	 */
    public class SpecularDot3Shader extends AbstractShader
    {
        /** @private */
		arcane override function updateMaterial(source:Object3D, view:View3D):void
        {
        	if (_bitmapDirty)
        		invalidateFaces();
        	
        	var _source_lightarray_directionals:Array = source.lightarray.directionals;
			var directional:DirectionalLight;
        	for each (directional in _source_lightarray_directionals) {
        		if (!directional.specularTransform[source])
        			directional.specularTransform[source] = new Dictionary(true);
        		
        		if (!directional.specularTransform[source][view] || view.scene.updatedObjects[source] || view.updated) {
        			directional.setSpecularTransform(source, view);
        			directional.setNormalMatrixSpecularTransform(source, view, _specular, _shininess);
        			updateFaces(source, view);
        		}
        	}
        }
		/** @private */
		arcane override function renderLayer(tri:DrawTriangle, layer:Sprite, level:int):int
        {
        	super.renderLayer(tri, layer, level);
        	
        	var _lights_directionals:Array = _lights.directionals;
			var directional:DirectionalLight;
        	for each (directional in _lights_directionals)
        	{
				_shape = _session.getLightShape(this, level++, layer, directional);
        		_shape.filters = [directional.normalMatrixSpecularTransform[_source][_view]];
        		_shape.blendMode = blendMode;
        		_graphics = _shape.graphics;
        		
				_source.session.renderTriangleBitmap(_bitmap, getUVData(tri), tri.screenVertices, tri.screenIndices, tri.startIndex, tri.endIndex, smooth, false, _graphics);
        	}
			
			if (debug)
                _source.session.renderTriangleLine(0, 0x0000FF, 1, tri.screenVertices, tri.screenCommands, tri.screenIndices, tri.startIndex, tri.endIndex);
            
            return level;
        }
        
        private var _zeroPoint:Point = new Point(0, 0);
        private var _bitmap:BitmapData;
        private var _sourceDictionary:Dictionary = new Dictionary(true);
        private var _sourceBitmap:BitmapData;
        private var _normalDictionary:Dictionary = new Dictionary(true);
        private var _normalBitmap:BitmapData;
        private var _shininess:Number;
		private var _specular:uint;
		private var _specularTransform:MatrixAway3D;
		private var _szx:Number;
		private var _szy:Number;
		private var _szz:Number;
		private var _normal0z:Number;
		private var _normal1z:Number;
		private var _normal2z:Number;
		private var _bitmapDirty:Boolean;
		
		protected override function calcUVT(tri:DrawTriangle, uvt:Vector.<Number>):Vector.<Number>
		{
			uvt[2] = 1/(_focus + tri.v0z);
			uvt[5] = 1/(_focus + tri.v1z);
			uvt[8] = 1/(_focus + tri.v2z);
			uvt[0] = tri.uv0.u;
    		uvt[1] = 1 - tri.uv0.v;
    		uvt[3] = tri.uv1.u;
    		uvt[4] = 1 - tri.uv1.v;
    		uvt[6] = tri.uv2.u;
    		uvt[7] = 1 - tri.uv2.v;
    		
    		return uvt;
		}
		
		/**
		 * @inheritDoc
		 */
        public function updateFaces(source:Object3D = null, view:View3D = null):void
        {
        	notifyMaterialUpdate();
        	
        	for each (var faceMaterialVO:FaceMaterialVO in _faceDictionary)
        		if (source == faceMaterialVO.source && view == faceMaterialVO.view)
	        		if (!faceMaterialVO.cleared)
	        			faceMaterialVO.clear();
        }
        
		/**
		 * @inheritDoc
		 */
        public function invalidateFaces(source:Object3D = null, view:View3D = null):void
        {
        	source; view;
        	
        	_bitmapDirty = false;
        	
        	for each (var faceMaterialVO:FaceMaterialVO in _faceDictionary)
        		faceMaterialVO.invalidated = true;
        }
        
		/**
		 * @inheritDoc
		 */
        protected override function renderShader(tri:DrawTriangle):void
        {
			//check to see if sourceDictionary exists
			_sourceBitmap = _sourceDictionary[tri];
			if (!_sourceBitmap || _faceMaterialVO.resized) {
				_sourceBitmap = _sourceDictionary[tri] = _parentFaceMaterialVO.bitmap.clone();
				_sourceBitmap.lock();
			}
			
			//check to see if normalDictionary exists
			_normalBitmap = _normalDictionary[tri];
			if (!_normalBitmap || _faceMaterialVO.resized) {
				_normalBitmap = _normalDictionary[tri] = _parentFaceMaterialVO.bitmap.clone();
				_normalBitmap.lock();
			}
			
			_n0 = _source.geometry.getVertexNormal(_face.v0);
			_n1 = _source.geometry.getVertexNormal(_face.v1);
			_n2 = _source.geometry.getVertexNormal(_face.v2);
			
			var _source_lightarray_directionals:Array = _source.lightarray.directionals;
			
			var directional:DirectionalLight;
			
			for each (directional in _source_lightarray_directionals)
	    	{
				_specularTransform = directional.specularTransform[_source];
				 
				_szx = _specularTransform.szx;
				_szy = _specularTransform.szy;
				_szz = _specularTransform.szz;
				
				_normal0z = _n0.x * _szx + _n0.y * _szy + _n0.z * _szz;
				_normal1z = _n1.x * _szx + _n1.y * _szy + _n1.z * _szz;
				_normal2z = _n2.x * _szx + _n2.y * _szy + _n2.z * _szz;
				
				//check to see if the uv triangle lies inside the bitmap area
				if (_normal0z > -0.2 || _normal1z > -0.2 || _normal2z > -0.2) {
					
					//store a clone
					if (_faceMaterialVO.cleared && !_parentFaceMaterialVO.updated) {
						_faceMaterialVO.bitmap = _parentFaceMaterialVO.bitmap.clone();
						_faceMaterialVO.bitmap.lock();
					}
					
					//update booleans
					_faceMaterialVO.cleared = false;
					_faceMaterialVO.updated = true;
					
					//resolve normal map
					_normalBitmap.applyFilter(_bitmap, _faceVO.face.bitmapRect, _zeroPoint, directional.normalMatrixSpecularTransform[_source][_view]);
		            
					//draw into faceBitmap
					_faceMaterialVO.bitmap.draw(_normalBitmap, null, directional.diffuseColorTransform, blendMode);
				}
	    	}
        }
        
        //TODO: implement tangent space option
        /**
        * Determines if the DOT3 mapping is rendered in tangent space (true) or object space (false).
        */
        public var tangentSpace:Boolean;
        
        /**
        * Returns the width of the bitmapData being used as the shader DOT3 map.
        */
        public function get width():Number
        {
            return _bitmap.width;
        }
        
        /**
        * Returns the height of the bitmapData being used as the shader DOT3 map.
        */
        public function get height():Number
        {
            return _bitmap.height;
        }
        
        /**
        * Returns the bitmapData object being used as the shader DOT3 map.
        */
        public function get bitmap():BitmapData
        {
        	return _bitmap;
        }
        
        public function set bitmap(val:BitmapData):void
        {
        	_bitmap = val;
        	
        	_bitmapDirty = true;
        }
        
		/**
		 * The exponential dropoff value used for specular highlights.
		 */
        public function get shininess():Number
        {
        	return _shininess;
        }
		
        public function set shininess(val:Number):void
        {
        	_shininess = val;
        }
		
		/**
		 * Coefficient for specular light level.
		 */
		public function get specular():uint
		{
			return _specular;
		}
		
		public function set specular(val:uint):void
		{
			_specular = val;
		}
		
        /**
        * Returns the argb value of the bitmapData pixel at the given u v coordinate.
        * 
        * @param	u	The u (horizontal) texture coordinate.
        * @param	v	The v (verical) texture coordinate.
        * @return		The argb pixel value.
        */
        public function getPixel32(u:Number, v:Number):uint
        {
        	return _bitmap.getPixel32(u*_bitmap.width, (1 - v)*_bitmap.height);
        }
		
		/**
		 * Creates a new <code>SpecularDot3Shader</code> object.
		 * 
		 * @param	bitmap			The bitmapData object to be used as the material's DOT3 map.
		 * @param	init	[optional]	An initialisation object for specifying default instance properties.
		 */
        public function SpecularDot3Shader(bitmap:BitmapData, init:Object = null)
        {
            super(init);
            
			_bitmap = bitmap;
			
            shininess = ini.getNumber("shininess", 20);
            specular = ini.getColor("specular", 0xFFFFFF);
            tangentSpace = ini.getBoolean("tangentSpace", false);
        }
    }
}
