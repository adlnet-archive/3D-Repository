package away3d.materials.shaders
{
	import away3d.arcane;
	import away3d.cameras.lenses.*;
	import away3d.containers.*;
	import away3d.core.base.*;
	import away3d.core.draw.*;
	import away3d.core.light.*;
	import away3d.core.math.*;
	import away3d.core.render.*;
	import away3d.core.utils.*;
	import away3d.events.*;
	import away3d.materials.*;
	
	import flash.display.*;
	import flash.events.*;
	import flash.geom.*;
	import flash.utils.*;	
	
	use namespace arcane;
	
	/**
	 * Base class for shaders.
    * Not intended for direct use - use one of the shading materials in the materials package.
    */
    public class AbstractShader extends LayerMaterial
    {
        /** @private */
        arcane var _faceDictionary:Dictionary = new Dictionary(true);
        /** @private */
        arcane var _sprite:Sprite;
        /** @private */
        arcane var _shape:Shape;
        /** @private */
		arcane var eTri0x:Number;
        /** @private */
		arcane var eTri0y:Number;
        /** @private */
		arcane var eTri1x:Number;
        /** @private */
		arcane var eTri1y:Number;
        /** @private */
		arcane var eTri2x:Number;
        /** @private */
		arcane var eTri2y:Number;
        /** @private */
        arcane var _s:Shape = new Shape();
        /** @private */
		arcane var _graphics:Graphics;
        /** @private */
		arcane var _bitmapRect:Rectangle;
        /** @private */
		arcane var _source:Mesh;
        /** @private */
		arcane var _session:AbstractRenderSession;
        /** @private */
		arcane var _view:View3D;
        /** @private */
		arcane var _face:Face;
		/** @private */
		arcane var _faceVO:FaceVO;
        /** @private */
		arcane var _lights:ILightConsumer;
        /** @private */
		arcane var _parentFaceMaterialVO:FaceMaterialVO;
        /** @private */
		arcane var _n0:Number3D;
        /** @private */
		arcane var _n1:Number3D;
        /** @private */
		arcane var _n2:Number3D;
        /** @private */
        arcane var _dict:Dictionary;
        /** @private */
		arcane var ambient:AmbientLight;
        /** @private */
		arcane var directional:DirectionalLight;
        /** @private */
		arcane var _faceMaterialVO:FaceMaterialVO;
        /** @private */
		arcane var _normal0:Number3D = new Number3D();
        /** @private */
		arcane var _normal1:Number3D = new Number3D();
        /** @private */
		arcane var _normal2:Number3D = new Number3D();
        /** @private */
		arcane var _map:Matrix = new Matrix();
		/** @private */
		arcane var _uvt:Vector.<Number> = new Vector.<Number>(9, true);
		/** @private */
		arcane var _focus:Number;
		/** @private */
		arcane var _mapping:Matrix;
        /** @private */
		arcane final function contains(v0x:Number, v0y:Number, v1x:Number, v1y:Number, v2x:Number, v2y:Number, x:Number, y:Number):Boolean
        {   
            if (v0x*(y - v1y) + v1x*(v0y - y) + x*(v1y - v0y) < -0.001)
                return false;

            if (v0x*(v2y - y) + x*(v0y - v2y) + v2x*(y - v0y) < -0.001)
                return false;

            if (x*(v2y - v1y) + v1x*(y - v2y) + v2x*(v1y - y) < -0.001)
                return false;

            return true;
        }
        /** @private */
        arcane override function renderLayer(tri:DrawTriangle, layer:Sprite, level:int):int
        {
        	_source = tri.source as Mesh;
        	_session = _source.session;
			_view = tri.view;
			_faceVO = tri.faceVO;
			_face = _faceVO.face;
			_lights = tri.source.lightarray;
			
			return level;
        }
        
		/** @private */
        arcane override function renderBitmapLayer(tri:DrawTriangle, containerRect:Rectangle, parentFaceMaterialVO:FaceMaterialVO):FaceMaterialVO
        {
        	_source = tri.source as Mesh;
        	_session = _source.session;
			_view = tri.view;
			_faceVO = tri.faceVO;
			_face = _faceVO.face;
			_parentFaceMaterialVO = parentFaceMaterialVO;
			
			_faceMaterialVO = getFaceMaterialVO(_faceVO, _source, _view);
			
			//pass on inverse texturemapping
			_faceMaterialVO.invtexturemapping = parentFaceMaterialVO.invtexturemapping;
			
			//pass on resize value
			if (parentFaceMaterialVO.resized) {
				parentFaceMaterialVO.resized = false;
				_faceMaterialVO.resized = true;
			}
			
			//check to see if rendering can be skipped
			if (parentFaceMaterialVO.updated || _faceMaterialVO.invalidated || _faceMaterialVO.updated) {
				parentFaceMaterialVO.updated = false;
				
				//retrieve the bitmapRect
				_bitmapRect = _faceVO.face.bitmapRect;
				
				//reset booleans
				if (_faceMaterialVO.invalidated)
					_faceMaterialVO.invalidated = false;
				else 
					_faceMaterialVO.updated = true;
				
				//store a clone
				_faceMaterialVO.bitmap = parentFaceMaterialVO.bitmap;
				
				//draw shader
				renderShader(tri);
			}
			
			return _faceMaterialVO;
        }
		/** @private */
        arcane function getFaceMaterialVO(faceVO:FaceVO, source:Object3D = null, view:View3D = null):FaceMaterialVO
        {
        	source;view;//TODO : FDT Warning
        	if ((_faceMaterialVO = _faceDictionary[faceVO]))
        		return _faceMaterialVO;
        	
        	return _faceDictionary[faceVO] = new FaceMaterialVO(source, view);
        }
        
        /**
        * Renders the shader to the specified face.
        * 
        * @param	face	The face object being rendered.
        */
        protected function renderShader(tri:DrawTriangle):void
        {
        	throw new Error("Not implemented");
        }
        
        protected function calcMapping(tri:DrawTriangle, map:Matrix):Matrix
        {
        	tri; map;
        	
        	map.a = 1;
			map.b = 0;
			map.c = 0;
			map.d = 1;
			map.tx = 0;
			map.ty = 0;
            map.invert();
            
            return map;
        }
        
        protected function calcUVT(tri:DrawTriangle, uvt:Vector.<Number>):Vector.<Number>
        {
        	tri; uvt;
        	
			uvt[0] = 0;
    		uvt[1] = 1;
    		uvt[3] = 0;
    		uvt[4] = 0;
    		uvt[6] = 1;
    		uvt[7] = 0;
    		
    		return uvt;
        }
        
        /**
        * Calculates the mapping matrix required to draw the triangle texture to screen.
        * 
        * @param	tri		The data object holding all information about the triangle to be drawn.
        * @return			The required matrix object.
        */
		protected function getMapping(tri:DrawTriangle):Matrix
		{
			if (tri.generated)
				return calcMapping(tri, _map);
			
			_faceMaterialVO = getFaceMaterialVO(tri.faceVO);
			
			if (!_faceMaterialVO.invalidated)
				return _faceMaterialVO.texturemapping;
			
			_faceMaterialVO.invalidated = false;
			
			return calcMapping(tri, _faceMaterialVO.texturemapping);
		}
		
		protected function getUVData(tri:DrawTriangle):Vector.<Number>
		{
			_faceMaterialVO = getFaceMaterialVO(tri.faceVO, tri.source, tri.view);
			
			if (_view.camera.lens is ZoomFocusLens)
        		_focus = tri.view.camera.focus;
        	else
        		_focus = 0;
			
			_faceMaterialVO.invalidated = false;
			//if (tri.generated) {
				_uvt[2] = 1/(_focus + tri.v0z);
				_uvt[5] = 1/(_focus + tri.v1z);
				_uvt[8] = 1/(_focus + tri.v2z);
				
	    		return calcUVT(tri, _uvt);
			//}
			/*
			_faceMaterialVO.uvtData[2] = 1/(_focus + tri.v0z);
			_faceMaterialVO.uvtData[5] = 1/(_focus + tri.v1z);
			_faceMaterialVO.uvtData[8] = 1/(_focus + tri.v2z);
			
			if (!_faceMaterialVO.invalidated)
				return _faceMaterialVO.uvtData;
			
        	
			return calcUVT(tri, _faceMaterialVO.uvtData);
			*/
		}
		
    	/**
    	 * Determines if the shader bitmap is smoothed (bilinearly filtered) when drawn to screen
    	 */
        public var smooth:Boolean;
        
        /**
        * Defines a blendMode value for the shader bitmap.
        */
        public var blendMode:String;
        
		/**
		 * Creates a new <code>AbstractShader</code> object.
		 * 
		 * @param	init	[optional]	An initialisation object for specifying default instance properties.
		 */
        public function AbstractShader(init:Object = null)
        {
            super(init);
            
            smooth = ini.getBoolean("smooth", false);
            blendMode = ini.getString("blendMode", BlendMode.NORMAL);
        }
    }
}
