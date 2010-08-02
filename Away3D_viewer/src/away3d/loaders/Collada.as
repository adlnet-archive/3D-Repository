﻿package away3d.loaders
{
	import away3d.animators.*;
	import away3d.animators.data.*;
	import away3d.arcane;
	import away3d.containers.*;
	import away3d.core.base.*;
	import away3d.core.math.*;
	import away3d.core.utils.*;
	import away3d.loaders.data.*;
	import away3d.loaders.utils.*;
	
	import flash.utils.*;
	
	use namespace arcane;
	
    /**
    * File loader for the Collada file format with animation.
    */
    public class Collada extends AbstractParser
    {
    	
        private var collada:XML;
        private var animationLibrary:AnimationLibrary;
        private var channelLibrary:ChannelLibrary;
        private var yUp:Boolean;
        private var toRADIANS:Number = Math.PI / 180;
		private var rotationMatrix:MatrixAway3D = new MatrixAway3D();
    	private var scalingMatrix:MatrixAway3D = new MatrixAway3D();
    	private var translationMatrix:MatrixAway3D = new MatrixAway3D();
        private var VALUE_X:String;
        private var VALUE_Y:String;
        private var VALUE_Z:String;
        private var VALUE_U:String = "S";
        private var VALUE_V:String = "T";
		private var _geometryArray:Array;
		private var _geometryArrayLength:int;
		private var _channelArray:Array;
		private var _channelArrayLength:int;
		private var _defaultAnimationClip:AnimationData;
		private var _haveClips:Boolean = false;
		private var _skinControllers:Array = [];
		private var _skinController:SkinController;
		
		private function buildAnimations():void
		{
			var bone:Bone;
			
			//hook up bones to skincontrollers
			for each (_skinController in _skinControllers) {
					bone = (_container as ObjectContainer3D).getBoneByName(_skinController.name);
	                if (bone) {
	                    _skinController.joint = bone.joint;
	                    bone.controller = _skinController;
						//_skinController.update();
	                } else {
	                	Debug.warning("no joint found for " + _skinController.name);
	                }
	  		}
		   			
			for each (var _animationData:AnimationData in animationLibrary)
			{
				switch (_animationData.animationType)
				{
					case AnimationDataType.SKIN_ANIMATION:
						var animator:BonesAnimator = new BonesAnimator();
						animator.target = _container;
						
						var param:Array;
			            var rX:String;
			            var rY:String;
			            var rZ:String;
			            var sX:String;
			            var sY:String;
			            var sZ:String;
						
						for each (var channelData:ChannelData in _animationData.channels) {
							var channel:Channel = channelData.channel;
							
							//channel.target = _containers[channel.name];
							animator.addChannel(channel);
							
							var times:Array = channel.times;
							
							if (_animationData.start > times[0])
								_animationData.start = times[0];
							
							if (_animationData.end < times[times.length-1])
								_animationData.end = times[times.length - 1];
							
				            if (channel.target is Bone) {
				            	rX = "jointRotationX";
				            	rY = "jointRotationY";
				            	rZ = "jointRotationZ";
				            	sX = "jointScaleX";
				            	sY = "jointScaleY";
				            	sZ = "jointScaleZ";
				            } else {
				            	rX = "rotationX";
				            	rY = "rotationY";
				            	rZ = "rotationZ";
				            	sX = "scaleX";
				            	sY = "scaleY";
				            	sZ = "scaleZ";
				            }
				            
				            switch(channelData.type)
				            {
				                case "translateX":
				                case "translationX":
								case "transform(3)(0)":
				                	channel.type = ["x"];
									if (yUp)
										for each (param in channel.param)
											param[0] *= -1*scaling;
				                	break;
								case "translateY":
								case "translationY":
								case "transform(3)(1)":
									if (yUp)
										channel.type = ["y"];
									else
										channel.type = ["z"];
									for each (param in channel.param)
										param[0] *= scaling;
				     				break;
								case "translateZ":
								case "translationZ":
								case "transform(3)(2)":
									if (yUp)
										channel.type = ["z"];
									else
										channel.type = ["y"];
									for each (param in channel.param)
										param[0] *= scaling;
				     				break;
				     			case "jointOrientX":
				     				channel.type = ["rotationX"];
				     				if (yUp)
										for each (param in channel.param)
											param[0] *= -1;
				     				break;
								case "rotateXANGLE":
								case "rotateX":
								case "RotX":
				     				channel.type = [rX];
				     				if (yUp)
										for each (param in channel.param)
											param[0] *= -1;
				     				break;
				     			case "jointOrientY":
				     				channel.type = ["rotationY"];
				     				//if (yUp)
										for each (param in channel.param)
											param[0] *= -1;
				     				break;
								case "rotateYANGLE":
								case "rotateY":
								case "RotY":
									if (yUp)
										channel.type = [rY];
									else
										channel.type = [rZ];
									//if (yUp)
										for each (param in channel.param)
											param[0] *= -1;
				     				break;
				     			case "jointOrientZ":
				     				channel.type = ["rotationZ"];
				     				//if (yUp)
										for each (param in channel.param)
											param[0] *= -1;
				     				break;
								case "rotateZANGLE":
								case "rotateZ":
								case "RotZ":
									if (yUp)
										channel.type = [rZ];
									else
										channel.type = [rY];
									//if (yUp)
										for each (param in channel.param)
											param[0] *= -1;
				            		break;
								case "scaleX":
								case "transform(0)(0)":
									channel.type = [sX];
									//if (yUp)
									//	for each (param in channel.param)
									//		param[0] *= -1;
				            		break;
								case "scaleY":
								case "transform(1)(1)":
									if (yUp)
										channel.type = [sY];
									else
										channel.type = [sZ];
				     				break;
								case "scaleZ":
								case "transform(2)(2)":
									if (yUp)
										channel.type = [sZ];
									else
										channel.type = [sY];
				     				break;
								case "translate":
								case "translation":
									if (yUp) {
										channel.type = ["x", "y", "z"];
										for each (param in channel.param)
											param[0] *= -1;
				     				} else {
				     					channel.type = ["x", "z", "y"];
				     				}
				     				for each (param in channel.param) {
										param[0] *= scaling;
										param[1] *= scaling;
										param[2] *= scaling;
				     				}
									break;
								case "scale":
									if (yUp)
										channel.type = [sX, sY, sZ];
									else
										channel.type = [sX, sZ, sY];
				     				break;
								case "rotate":
									if (yUp) {
										channel.type = [rX, rY, rZ];
										for each (param in channel.param) {
											param[0] *= -1;
											param[1] *= -1;
											param[2] *= -1;
										}
				     				} else {
										channel.type = [rX, rZ, rY];
										for each (param in channel.param) {
											param[1] *= -1;
											param[2] *= -1;
										}
				     				}
									break;
								case "transform":
									channel.type = ["transform"];
									break;
								
								case "visibility":
									channel.type = ["visibility"];
									break;
				            }
						}
						
						animator.delay = _animationData.start;
						animator._totalFrames = (_animationData.end - _animationData.start)*animator.fps;
						
						_animationData.animator = animator;
						break;
					case AnimationDataType.VERTEX_ANIMATION:
						break;
				}
			}
		}
		
        private function getArray(spaced:String):Array
        {
        	spaced = spaced.split("\r\n").join(" ");
            var strings:Array = spaced.split(" ");
            var numbers:Array = [];
			
            var totalStrings:Number = strings.length;
			
            for (var i:Number = 0; i < totalStrings; ++i)
            	if (strings[i] != "")
                	numbers.push(Number(strings[i]));

            return numbers;
        }
		
        private function rotateMatrix(vector:Array):MatrixAway3D
        {
            if (yUp) {
                	rotationMatrix.rotationMatrix(vector[0], -vector[1], -vector[2], vector[3]*toRADIANS);
            } else {
                	rotationMatrix.rotationMatrix(vector[0], vector[2], vector[1], -vector[3]*toRADIANS);
            }
            
            return rotationMatrix;
        }

        private function translateMatrix(vector:Array):MatrixAway3D
        {
            if (yUp)
                translationMatrix.translationMatrix(-vector[0]*scaling, vector[1]*scaling, vector[2]*scaling);
            else
                translationMatrix.translationMatrix(vector[0]*scaling, vector[2]*scaling, vector[1]*scaling);
			
            return translationMatrix;
        }
		
        private function scaleMatrix(vector:Array):MatrixAway3D
        {
            if (yUp)
                scalingMatrix.scaleMatrix(vector[0], vector[1], vector[2]);
            else
                scalingMatrix.scaleMatrix(vector[0], vector[2], vector[1]);
			
            return scalingMatrix;
        }

        private function getId(url:String):String
        {
            return url.split("#")[1];
        }
        
        /** @private */
        protected override function getFileType():String
        {
        	return "Collada";
        }
        
    	/**
    	 * A scaling factor for all geometry in the model. Defaults to 1.
    	 */
        public var scaling:Number;
        
    	/**
    	 * Controls the use of shading materials when color textures are encountered. Defaults to false.
    	 */
        public var shading:Boolean;
		
		/**
		 * Creates a new <code>Collada</code> object.
		 *
		 * @param	init	[optional]	An initialisation object for specifying default instance properties.
		 *
		 * @see away3d.loaders.Collada#parse()
		 * @see away3d.loaders.Collada#load()
		 */
        public function Collada(init:Object = null)
        {
            super(init);
            
            scaling = ini.getNumber("scaling", 1);
            shading = ini.getBoolean("shading", false);
            centerMeshes = ini.getBoolean("centerMeshes", false);
			
			//create the container
            _container = new ObjectContainer3D(ini);
			_container.name = "collada";
			
			_container.materialLibrary = _materialLibrary;
			_container.geometryLibrary = _geometryLibrary;
			
			animationLibrary = _container.animationLibrary = new AnimationLibrary();
			channelLibrary = new ChannelLibrary();
			
			binary = false;
        }
		
        /**
		 * Creates a 3d container object from the raw xml data of a collada file.
		 *
		 * @param	data				The xml data of a loaded file.
		 * @param	init	[optional]	An initialisation object for specifying default instance properties.
		 * @param	loader	[optional]	Not intended for direct use.
		 *
		 * @return						A 3d loader object that can be used as a placeholder in a scene while the file is parsing.
		 */
        public static function parse(data:*, init:Object = null):ObjectContainer3D
        {
            return Loader3D.parse(data, Collada, init).handle as ObjectContainer3D;
        }
		
    	/**
    	 * Loads and parses a collada file into a 3d container object.
    	 *
    	 * @param	url					The url location of the file to load.
    	 * @param	init	[optional]	An initialisation object for specifying default instance properties.
    	 * 
    	 * @return						A 3d loader object that can be used as a placeholder in a scene while the file is loading.
    	 */
        public static function load(url:String, init:Object = null):Loader3D
        {
			return Loader3D.load(url, Collada, init);
        }
        
        /** @private */
        arcane override function prepareData(data:*):void
        {
        	collada = Cast.xml(data);
        	
			default xml namespace = collada.namespace();
			Debug.trace(" ! ------------- Begin Parse Collada -------------");

            // Get up axis
            yUp = (collada["asset"].up_axis == "Y_UP")||(String(collada["asset"].up_axis) == "");

    		if (yUp) {
    			VALUE_X = "X";
    			VALUE_Y = "Y";
    			VALUE_Z = "Z";
        	} else {
                VALUE_X = "X";
                VALUE_Y = "Z";
                VALUE_Z = "Y";
        	}
			
            parseScene();
			
			parseAnimationClips();
        }
    	/** @private */
        arcane override function parseNext():void
        {
        	if (_parsedChunks < _geometryArrayLength)
        		parseGeometryData(_geometryArray[_parsedChunks]);
        	else
        		parseChannelData(_channelArray[-_geometryArrayLength + _parsedChunks]);
        	
        	_parsedChunks++;
        	
        	if (_parsedChunks == _totalChunks) {
	        	//build materials
				buildMaterials();
				
				//build the containers
				buildContainers(_containerData, _container as ObjectContainer3D);
				
				//build animations
				buildAnimations();
				
	        	notifySuccess();
        	} else {
				notifyProgress();
	        }
        }
        		
		/**
		 * Converts the scene heirarchy to an Away3d data structure
		 */
        private function parseScene():void
        {
        	var scene:XML = collada["library_visual_scenes"].visual_scene.(@id == getId(collada["scene"].instance_visual_scene.@url))[0];
        	
        	if (scene == null) {
        		Debug.trace(" ! ------------- No scene to parse -------------");
        		return;
        	}
        	
			Debug.trace(" ! ------------- Begin Parse Scene -------------");
			
			_containerData = new ContainerData();
			
            for each (var node:XML in scene["node"])
				parseNode(node, _containerData);
			
			Debug.trace(" ! ------------- End Parse Scene -------------");
			_geometryArray = geometryLibrary.getGeometryArray();
			_geometryArrayLength = _geometryArray.length;
			_totalChunks += _geometryArrayLength;
		}
		
		/**
		 * Converts a single scene node to a BoneData ContainerData or MeshData object.
		 * 
		 * @see away3d.loaders.data.BoneData
		 * @see away3d.loaders.data.ContainerData
		 * @see away3d.loaders.data.MeshData
		 */
        private function parseNode(node:XML, parent:ContainerData):void
        {	
			var _transform:MatrixAway3D;
	    	var _objectData:ObjectData;
	    	
        	if (String(node["instance_light"].@url) != "" || String(node["instance_camera"].@url) != "")
        		return;
	    	
	    	
			if (String(node["instance_controller"]) == "" && String(node["instance_geometry"]) == "")
			{
				
				if (String(node.@type) == "JOINT")
					_objectData = new BoneData();
				else {
					if (String(node["instance_node"].@url) == "" && (String(node["node"]) == "" || parent is BoneData))
						return;
					_objectData = new ContainerData();
				}
			}else{
				_objectData = new MeshData();
			}
			
			parent.children.push(_objectData);
			
			//ColladaMaya 3.05B
			if (String(node.@type) == "JOINT")
				_objectData.id = node.@sid;
			else
				_objectData.id = node.@id;
			
			//ColladaMaya 3.02
            _objectData.name = node.@id;
            _transform = _objectData.transform;
			
			Debug.trace(" + Parse Node : " + _objectData.id + " : " + _objectData.name);
			
			var nodeName:String;
           	var geo:XML;
           	var ctrlr:XML;
           	var sid:String;
			var instance_material:XML;
			var arrayChild:Array;
			var boneData:BoneData = (_objectData as BoneData);
			
            for each (var childNode:XML in node.children())
            {
                arrayChild = getArray(childNode);
                nodeName = String(childNode.name()["localName"]);
				switch(nodeName)
                {
					case "translate":
                        _transform.multiply(_transform, translateMatrix(arrayChild));
                        
                        break;

                    case "rotate":
                    	sid = childNode.@sid;
                        if (_objectData is BoneData && (sid == "rotateX" || sid == "rotateY" || sid == "rotateZ" || sid == "rotX" || sid == "rotY" || sid == "rotZ"))
							boneData.jointTransform.multiply(boneData.jointTransform, rotateMatrix(arrayChild));
                        else
	                        _transform.multiply(_transform, rotateMatrix(arrayChild));
	                    
                        break;
						
                    case "scale":
                        if (_objectData is BoneData)
							boneData.jointTransform.multiply(boneData.jointTransform, scaleMatrix(arrayChild));
                        else
	                        _transform.multiply(_transform, scaleMatrix(arrayChild));
						
                        break;
						
                    // Baked transform matrix
                    case "matrix":
                    	var m:MatrixAway3D = new MatrixAway3D();
                    	m.array2matrix(arrayChild, yUp, scaling);
                        _transform.multiply(_transform, m);
						break;
						
                    case "node":
                    	//3dsMax 11 - Feeling ColladaMax v3.05B
                    	//<node><node/></node>
                    	if(_objectData is MeshData)
                    	{
							parseNode(childNode, parent as ContainerData);
                    	}else{
                    		parseNode(childNode, _objectData as ContainerData);
                    	}
                        
                        break;

    				case "instance_node":
    					parseNode(collada["library_nodes"].node.(@id == getId(childNode.@url))[0], _objectData as ContainerData);
    					
    					break;

                    case "instance_geometry":
                    	if(String(childNode).indexOf("lines") == -1) {
							
							//add materials to materialLibrary
	                        for each (instance_material in childNode..instance_material)
	                        	parseMaterial(instance_material.@symbol, getId(instance_material.@target));
							
							geo = collada["library_geometries"].geometry.(@id == getId(childNode.@url))[0];
							
							//parent.children.remove(_objectData);
							var _objectData2 = new MeshData();
							parent.children.push(_objectData2);
	                        (_objectData2 as MeshData).geometry = geometryLibrary.addGeometry(geo.@id, geo);
	                    }
	                    
                        break;
					
                    case "instance_controller":
						
						//add materials to materialLibrary
						for each (instance_material in childNode..instance_material)
							parseMaterial(instance_material.@symbol, getId(instance_material.@target));
						
						ctrlr = collada["library_controllers"].controller.(@id == getId(childNode.@url))[0];
						geo = collada["library_geometries"].geometry.(@id == getId(ctrlr["skin"][0].@source))[0];
						
	                    (_objectData as MeshData).geometry = geometryLibrary.addGeometry(geo.@id, geo, ctrlr);
						
						(_objectData as MeshData).skeleton = getId(childNode["skeleton"]);
						break;
                }
            }
        }
		
		/**
		 * Converts a material definition to a MaterialData object
		 * 
		 * @see away3d.loaders.data.MaterialData
		 */
        private function parseMaterial(symbol:String, materialName:String):void
        {
           	var _materialData:MaterialData = materialLibrary.addMaterial(materialName);
        	_symbolLibrary[symbol] = _materialData;
            if(symbol == "FrontColorNoCulling") {
            	_materialData.materialType = MaterialData.SHADING_MATERIAL;
            } else {
                _materialData.textureFileName = getTextureFileName(materialName);
                
                if (_materialData.textureFileName) {
            		_materialData.materialType = MaterialData.TEXTURE_MATERIAL;
                } else {
                	if (shading)
                		_materialData.materialType = MaterialData.SHADING_MATERIAL;
                	else
	                	_materialData.materialType = MaterialData.WIREFRAME_MATERIAL;
                	
                	//parseColorMaterial(materialName, _materialData);
                }
            }
        }
		
		/**
		 * Parses geometry data.
		 * 
		 * @see away3d.loaders.data.GeometryData
		 */
		private function parseGeometryData(geometryData:GeometryData):void
		{
			parseTristrips(geometryData);
			parseTriangles(geometryData);
		
		}
		private function veccopy(vec:FaceData):FaceData
		{
			var result:FaceData= new FaceData();
			result["v" + 0] = vec["v" + 0];
			result["v" + 1] = vec["v" + 1];
			result["v" + 2] = vec["v" + 2];

			result["uv" + 0] = vec["uv" + 0];
			result["uv" + 1] = vec["uv" + 1];
			result["uv" + 2] = vec["uv" + 2];
			return result;
		}
		private function parseTristrips(geometryData:GeometryData)
		{
			Debug.trace(" + Parse Geometry : "+ geometryData.name);
			var verticesDictionary:Dictionary = new Dictionary(true);
			
			// Triangles
			var tristripsXMLList:XMLList = geometryData.geoXML["mesh"].tristrips;
			
			// C4D
			var isC4D:Boolean = (tristripsXMLList.length()==0 && geometryData.geoXML["mesh"].polylist.length()>0);
			if(isC4D)
				tristripsXMLList = geometryData.geoXML["mesh"].polylist;
			
			for each (var triangles:XML in tristripsXMLList)
			{
				// Input
				var field:Array = [];
				
				for each(var input:XML in triangles["input"])
				{
					var semantic:String = input.@semantic;
					switch(semantic)
					{
						case "VERTEX":
							deserialize(input, geometryData.geoXML, Vertex, geometryData.vertices);
							break;
						case "TEXCOORD":
							deserialize(input, geometryData.geoXML, UV, geometryData.uvs);
							break;
						default:
					}
					field.push(input.@semantic);
				}
				
				for each(var p:XML in triangles["p"])
				{
					var data     :Array  = p.split(' ');
					var len      :Number = data.length;
					var symbol :String = triangles.@material;
					
					Debug.trace(" + Parse MeshMaterialData");
					var _meshMaterialData:MeshMaterialData = new MeshMaterialData();
					_meshMaterialData.symbol = symbol;
					geometryData.materials.push(_meshMaterialData);
					
					//if (!materialLibrary[material])
					//	parseMaterial(material, material);
					var j:Number =0;
					var firsttriangle:FaceData = null;
					var wind:Boolean = false;
					while(data.length > 0)
					{
						var _faceData:FaceData = new FaceData();
						if(firsttriangle == null)
						{
							for (var vn:Number = 0; vn < 3; vn++)
							{
								for each (var fld:String in field)
								{
									switch(fld)
									{
										case "VERTEX":
											_faceData["v" + vn] = data.shift();
											break;
										case "TEXCOORD":
											_faceData["uv" + vn] = data.shift();
											break;
										default:
											data.shift();
									}
								}
							}
						}else
						{
							_faceData["v" + 0] = firsttriangle["v" + 1];
							if(firsttriangle["uv" + 1])
								_faceData["uv" + 0] = firsttriangle["uv" + 1];
							_faceData["v" + 1] = firsttriangle["v" + 2];
							if(firsttriangle["uv" + 2])
								_faceData["uv" + 1] = firsttriangle["uv" + 2];
							
							for each (var fld:String in field)
							{
								switch(fld)
								{
									case "VERTEX":
										_faceData["v" + 2] = data.shift();
										break;
									case "TEXCOORD":
										_faceData["uv" + 2] = data.shift();
										break;
									default:
										data.shift();
								}
							}
						}
						
						firsttriangle = veccopy(_faceData);
						
						if(wind)
						{
							var back = _faceData["v" + 0];
							_faceData["v" + 0] = _faceData["v" + 2];
							_faceData["v" + 2] = back;
							
							if(_faceData["uv" + 0])
							{
								var back2 = _faceData["uv" + 0];
								_faceData["uv" + 0] = _faceData["uv" + 2];
								_faceData["uv" + 2] = back2;
							}
							
						}
						wind = wind == false;
						
						verticesDictionary[_faceData.v0] = geometryData.vertices[_faceData.v0];
						verticesDictionary[_faceData.v1] = geometryData.vertices[_faceData.v1];
						verticesDictionary[_faceData.v2] = geometryData.vertices[_faceData.v2];
						
						_meshMaterialData.faceList.push(geometryData.faces.length);
						
						
						geometryData.faces.push(_faceData);
						j++;
					}
				}
			}
			
			//center vertex points in mesh for better bounding radius calulations
			if (centerMeshes) {
				geometryData.maxX = -Infinity;
				geometryData.minX = Infinity;
				geometryData.maxY = -Infinity;
				geometryData.minY = Infinity;
				geometryData.maxZ = -Infinity;
				geometryData.minZ = Infinity;
				for each (var _vertex:Vertex in verticesDictionary) {
					if (geometryData.maxX < _vertex._x)
						geometryData.maxX = _vertex._x;
					if (geometryData.minX > _vertex._x)
						geometryData.minX = _vertex._x;
					if (geometryData.maxY < _vertex._y)
						geometryData.maxY = _vertex._y;
					if (geometryData.minY > _vertex._y)
						geometryData.minY = _vertex._y;
					if (geometryData.maxZ < _vertex._z)
						geometryData.maxZ = _vertex._z;
					if (geometryData.minZ > _vertex._z)
						geometryData.minZ = _vertex._z;
				}
			}
			
			// Double Side
			if (String(geometryData.geoXML["extra"].technique.double_sided) != "")
				geometryData.bothsides = (geometryData.geoXML["extra"].technique.double_sided[0].toString() == "1");
			else
				geometryData.bothsides = false;
			
			//parse controller
			if (!geometryData.ctrlXML)
				return;
			
			var skin:XML = geometryData.ctrlXML["skin"][0];
			
			var jointId:String = getId(skin["joints"].input.(@semantic == "JOINT")[0].@source);
			var tmp:String = skin["source"].(@id == jointId)["Name_array"].toString();
			//Blender?
			if (!tmp) tmp = skin["source"].(@id == jointId)["IDREF_array"].toString();
			tmp = tmp.replace(/\n/g, " ");
			var nameArray:Array = tmp.split(" ");
			
			var bind_shape:MatrixAway3D = new MatrixAway3D();
			bind_shape.array2matrix(getArray(skin["bind_shape_matrix"][0].toString()), yUp, scaling);
			
			var bindMatrixId:String = getId(skin["joints"].input.(@semantic == "INV_BIND_MATRIX").@source);
			var float_array:Array = getArray(skin["source"].(@id == bindMatrixId)[0].float_array.toString());
			
			var v:Array;
			var matrix:MatrixAway3D;
			var name:String;
			var skinController:SkinController;
			var i:int = 0;
			
			while (i < float_array.length)
			{
				name = nameArray[i / 16];
				matrix = new MatrixAway3D();
				matrix.array2matrix(float_array.slice(i, i+16), yUp, scaling);
				matrix.multiply(matrix, bind_shape);
				
				geometryData.skinControllers.push(skinController = new SkinController());
				skinController.name = name;
				skinController.bindMatrix = matrix;
				_skinControllers.push(skinController);
				i = i + 16;
			}
			
			Debug.trace(" + SkinWeight");
			
			tmp = skin["vertex_weights"][0].@count;
			var weightsId:String = getId(skin["vertex_weights"].input.(@semantic == "WEIGHT")[0].@source);
			
			tmp = skin["source"].(@id == weightsId)["float_array"].toString();
			var weights:Array = tmp.split(" ");
			
			tmp = skin["vertex_weights"].vcount.toString();
			var vcount:Array = tmp.split(" ");
			
			tmp = skin["vertex_weights"].v.toString();
			v = tmp.split(" ");
			
			var skinVertex	:SkinVertex;
			var c			:int;
			var count		:int = 0;
			
			i=0;
			while (i < geometryData.vertices.length)
			{
				c = int(vcount[i]);
				skinVertex = new SkinVertex(geometryData.vertices[i]);
				geometryData.vertices[i].skinVertex = skinVertex;
				geometryData.skinVertices.push(skinVertex);
				
				for each (skinController in geometryData.skinControllers)
				skinController.skinVertices.push(skinVertex);
				
				j=0;
				while (j < c)
				{
					skinVertex.controllers.push(geometryData.skinControllers[int(v[count])]);
					count++;
					skinVertex.weights.push(Number(weights[int(v[count])]));
					count++;
					++j;
				}
				++i;
			}
		}
		public function parseTriangles(geometryData:GeometryData):void{
			Debug.trace(" + Parse Geometry : "+ geometryData.name);
			var verticesDictionary:Dictionary = new Dictionary(true);
			
			// Triangles
			var trianglesXMLList:XMLList = geometryData.geoXML["mesh"].triangles;
			
			// C4D
			var isC4D:Boolean = (trianglesXMLList.length()==0 && geometryData.geoXML["mesh"].polylist.length()>0);
			if(isC4D)
				trianglesXMLList = geometryData.geoXML["mesh"].polylist;
			
			for each (var triangles:XML in trianglesXMLList)
			{
				// Input
				var field:Array = [];
				
				for each(var input:XML in triangles["input"])
				{
					var semantic:String = input.@semantic;
					switch(semantic)
					{
						case "VERTEX":
							deserialize(input, geometryData.geoXML, Vertex, geometryData.vertices);
							break;
						case "TEXCOORD":
							deserialize(input, geometryData.geoXML, UV, geometryData.uvs);
							break;
						default:
					}
					field.push(input.@semantic);
				}
				
				var data     :Array  = triangles["p"].split(' ');
				var len      :Number = triangles.@count;
				var symbol :String = triangles.@material;
				
				Debug.trace(" + Parse MeshMaterialData");
				var _meshMaterialData:MeshMaterialData = new MeshMaterialData();
				_meshMaterialData.symbol = symbol;
				geometryData.materials.push(_meshMaterialData);
				
				//if (!materialLibrary[material])
				//	parseMaterial(material, material);
				
				for (var j:Number = 0; j < len; ++j)
				{
					var _faceData:FaceData = new FaceData();
					
					for (var vn:Number = 0; vn < 3; vn++)
					{
						for each (var fld:String in field)
						{
							switch(fld)
							{
								case "VERTEX":
									_faceData["v" + vn] = data.shift();
									break;
								case "TEXCOORD":
									_faceData["uv" + vn] = data.shift();
									break;
								default:
									data.shift();
							}
						}
					}
					
					verticesDictionary[_faceData.v0] = geometryData.vertices[_faceData.v0];
					verticesDictionary[_faceData.v1] = geometryData.vertices[_faceData.v1];
					verticesDictionary[_faceData.v2] = geometryData.vertices[_faceData.v2];
					
					_meshMaterialData.faceList.push(geometryData.faces.length);
					geometryData.faces.push(_faceData);
				}
			}
			
			//center vertex points in mesh for better bounding radius calulations
			if (centerMeshes) {
				geometryData.maxX = -Infinity;
				geometryData.minX = Infinity;
				geometryData.maxY = -Infinity;
				geometryData.minY = Infinity;
				geometryData.maxZ = -Infinity;
				geometryData.minZ = Infinity;
				for each (var _vertex:Vertex in verticesDictionary) {
					if (geometryData.maxX < _vertex._x)
						geometryData.maxX = _vertex._x;
					if (geometryData.minX > _vertex._x)
						geometryData.minX = _vertex._x;
					if (geometryData.maxY < _vertex._y)
						geometryData.maxY = _vertex._y;
					if (geometryData.minY > _vertex._y)
						geometryData.minY = _vertex._y;
					if (geometryData.maxZ < _vertex._z)
						geometryData.maxZ = _vertex._z;
					if (geometryData.minZ > _vertex._z)
						geometryData.minZ = _vertex._z;
				}
			}
			
			// Double Side
			if (String(geometryData.geoXML["extra"].technique.double_sided) != "")
				geometryData.bothsides = (geometryData.geoXML["extra"].technique.double_sided[0].toString() == "1");
			else
				geometryData.bothsides = false;
			
			//parse controller
			if (!geometryData.ctrlXML)
				return;
			
			var skin:XML = geometryData.ctrlXML["skin"][0];
			
			var jointId:String = getId(skin["joints"].input.(@semantic == "JOINT")[0].@source);
			var tmp:String = skin["source"].(@id == jointId)["Name_array"].toString();
			//Blender?
			if (!tmp) tmp = skin["source"].(@id == jointId)["IDREF_array"].toString();
			tmp = tmp.replace(/\n/g, " ");
			var nameArray:Array = tmp.split(" ");
			
			var bind_shape:MatrixAway3D = new MatrixAway3D();
			bind_shape.array2matrix(getArray(skin["bind_shape_matrix"][0].toString()), yUp, scaling);
			
			var bindMatrixId:String = getId(skin["joints"].input.(@semantic == "INV_BIND_MATRIX").@source);
			var float_array:Array = getArray(skin["source"].(@id == bindMatrixId)[0].float_array.toString());
			
			var v:Array;
			var matrix:MatrixAway3D;
			var name:String;
			var skinController:SkinController;
			var i:int = 0;
			
			while (i < float_array.length)
			{
				name = nameArray[i / 16];
				matrix = new MatrixAway3D();
				matrix.array2matrix(float_array.slice(i, i+16), yUp, scaling);
				matrix.multiply(matrix, bind_shape);
				
				geometryData.skinControllers.push(skinController = new SkinController());
				skinController.name = name;
				skinController.bindMatrix = matrix;
				_skinControllers.push(skinController);
				i = i + 16;
			}
			
			Debug.trace(" + SkinWeight");
			
			tmp = skin["vertex_weights"][0].@count;
			var weightsId:String = getId(skin["vertex_weights"].input.(@semantic == "WEIGHT")[0].@source);
			
			tmp = skin["source"].(@id == weightsId)["float_array"].toString();
			var weights:Array = tmp.split(" ");
			
			tmp = skin["vertex_weights"].vcount.toString();
			var vcount:Array = tmp.split(" ");
			
			tmp = skin["vertex_weights"].v.toString();
			v = tmp.split(" ");
			
			var skinVertex	:SkinVertex;
			var c			:int;
			var count		:int = 0;
			
			i=0;
			while (i < geometryData.vertices.length)
			{
				c = int(vcount[i]);
				skinVertex = new SkinVertex(geometryData.vertices[i]);
				geometryData.vertices[i].skinVertex = skinVertex;
				geometryData.skinVertices.push(skinVertex);
				
				for each (skinController in geometryData.skinControllers)
				skinController.skinVertices.push(skinVertex);
				
				j=0;
				while (j < c)
				{
					skinVertex.controllers.push(geometryData.skinControllers[int(v[count])]);
					count++;
					skinVertex.weights.push(Number(weights[int(v[count])]));
					count++;
					++j;
				}
				++i;
			}
		}
		
			
		
		/**
		 * Detects and parses all animation clips
		 */ 
		private function parseAnimationClips() : void
        {
			
        	//Check for animations
			var anims:XML = collada["library_animations"][0];
			
			if (!anims) {
        		Debug.trace(" ! ------------- No animations to parse -------------");
        		return;
			}
        	
			//Check to see if animation clips exist
			var clips:XML = collada["library_animation_clips"][0];
			
			Debug.trace(" ! Animation Clips Exist : " + _haveClips);
			
            Debug.trace(" ! ------------- Begin Parse Animation -------------");
            
            var _channel_id:uint = 0;
            
            //loop through all animation channels
            if(anims["animation"]["animation"].length()==0)
			for each (var channel:XML in anims["animation"])
			{
				if(String(channel.@id).length>0)
				{
					channelLibrary.addChannel(channel.@id, channel);
				}else{
					// COLLADAMax NextGen;  Version: 1.1.0;  Platform: Win32;  Configuration: Release Max2009
					// issue#1 : missing channel.@id -> use automatic id instead
					Debug.trace(" ! COLLADAMax2009 id : _"+_channel_id);
					channelLibrary.addChannel("_"+String(_channel_id++), channel);
				}
			}

			// C4D 
			// issue#1 : animation -> animation.animation
			// issue#2 : missing channel.@id -> use automatic id instead
			for each (channel in anims["animation"]["animation"])
			{
				if(String(channel.@id).length > 0)
				{
					channelLibrary.addChannel(channel.@id, channel);
				}else{
					Debug.trace(" ! C4D id : _"+_channel_id);
					channelLibrary.addChannel("_"+String(_channel_id++), channel);
				}
			}
					
			if (clips) {
				//loop through all animation clips
				for each (var clip:XML in clips["animation_clip"])
					parseAnimationClip(clip);
			}
			
			//create default animation clip
			_defaultAnimationClip = animationLibrary.addAnimation("default");
			_defaultAnimationClip.animationType = AnimationDataType.SKIN_ANIMATION;
			
			for each (var channelData:ChannelData in channelLibrary)
				_defaultAnimationClip.channels[channelData.name] = channelData;
			
			Debug.trace(" ! ------------- End Parse Animation -------------");
			_channelArray = channelLibrary.getChannelArray();
			_channelArrayLength = _channelArray.length;
			_totalChunks += _channelArrayLength;
        }
        
        private function parseAnimationClip(clip:XML) : void
        {
        	Debug.trace(" + Parse Animation : " + clip.@id);
        	
			var animationClip:AnimationData = animationLibrary.addAnimation(clip.@id);
			animationClip.animationType = AnimationDataType.SKIN_ANIMATION;
			
			for each (var channel:XML in clip["instance_animation"])
				animationClip.channels[getId(channel.@url)] = channelLibrary[getId(channel.@url)];
        }
		
		private function parseChannelData(channelData:ChannelData) : void
        {
        	var node:XML = channelData.xml;
			var id:String = node["channel"].@target;
			var name:String = id.split("/")[0];
            var type:String = id.split("/")[1];
			var sampler:XML = node["sampler"][0];
			
            if (!type) {
            	Debug.trace(" ! No animation type detected");
            	return;
            }
            
            // C4D : didn't have @id, Maya 7 exporter has X/Y/Z split on translate
            if (String(node.@id).length > 0 && (type.split(".").length == 1 || type.split(".")[1].length > 1)) {
            	type = type.split(".")[0];
            	
            	if ((type == "image" || node.@id.split(".")[1] == "frameExtension")) {
	                //TODO : Material Animation
					Debug.trace(" ! Material animation not yet implemented");
					return;
            	}
            	
            } else {
            	type = type.split(".").join("");
            }
            
			

            
            var channel:Channel = channelData.channel = new Channel(name);
			var i:int;
			var j:int;
			
			_defaultAnimationClip.channels[channelData.name] = channelData;
			
			Debug.trace(" ! channelType : " + type);
			
            for each (var input:XML in sampler["input"])
            {
				var src:XML = node["source"].(@id == getId(input.@source))[0];
                var list:Array = String(src["float_array"]).split(" ");
                var len:int = int(src["technique_common"].accessor.@count);
                var stride:int = int(src["technique_common"].accessor.@stride);
                var semantic:String = input.@semantic;
				
				//C4D : no stride defined
				if (stride == 0)
					stride=1;
				
				var p:String;
				
                switch(semantic) {
                    case "INPUT":
                        for each (p in list)
                            channel.times.push(Number(p));
                        
                        if (_defaultAnimationClip.start > channel.times[0])
                            _defaultAnimationClip.start = channel.times[0];
                        
                        if (_defaultAnimationClip.end < channel.times[channel.times.length-1])
                            _defaultAnimationClip.end = channel.times[channel.times.length-1];
                        
                        break;
                    case "OUTPUT":
                        i=0;
                        while (i < len) {
                           channel.param[i] = [];
                            
                            if (stride == 16) {
		                    	var m:MatrixAway3D = new MatrixAway3D();
		                    	m.array2matrix(list.slice(i*stride, i*stride + 16), yUp, scaling);
		                    	channel.param[i].push(m);
                            } else {
	                            j = 0;
	                            while (j < stride) {
	                            	channel.param[i].push(Number(list[i*stride + j]));
	                            	++j;
	                            }
                            }
                            ++i;
                        }
                        Debug.trace("OUTPUT:"+len);
                        break;
                    case "INTERPOLATION":
                        for each (p in list)
                        {
							channel.interpolations.push(p);
                        }
                        break;
                    case "IN_TANGENT":
                        i=0;
                        while (i < len)
                        {
                        	channel.inTangent[i] = [];
                        	j = 0;
                            while (j < stride) {
                                channel.inTangent[i].push(new Number2D(Number(list[stride * i + j]), Number(list[stride * i + j + 1])));
                            	++j;
                            }
                            ++i;
                        }
                        break;
                    case "OUT_TANGENT":
                        i=0;
                        while (i < len)
                        {
                        	channel.outTangent[i] = [];
                        	j = 0;
                            while (j < stride) {
                                channel.outTangent[i].push(new Number2D(Number(list[stride * i + j]), Number(list[stride * i + j + 1])));
                            	++j;
                            }
                            ++i;
                        }
                        break;
                }
            }
            
			channelData.type = type;
        }
		
		/**
		 * Retrieves the filename of a material
		 */
		private function getTextureFileName( materialName:String ):String
		{
			var filename :String = null;
			var material:XML = collada["library_materials"].material.(@id == materialName)[0];
	
			if( material )
			{
				var effectId:String = getId( material["instance_effect"].@url );
				var effect:XML = collada["library_effects"].effect.(@id == effectId)[0];
	
				if (effect..texture.length() == 0) return null;
	
				var textureId:String = effect..texture[0].@texture;
	
				var sampler:XML =  effect..newparam.(@sid == textureId)[0];
	
				// Blender
				var imageId:String = textureId;
	
				// Not Blender
				if( sampler )
				{
					var sourceId:String = sampler..source[0];
					var source:XML =  effect..newparam.(@sid == sourceId)[0];
	
					imageId = source..init_from[0];
				}
	
				var image:XML = collada["library_images"].image.(@id == imageId)[0];
				if(!image)
				{
					image = collada["library_effects"].image.(@id == imageId)[0];
				}
				//3dsMax 11 - Feeling ColladaMax v3.05B.
				if(!image)
				{
					filename = effect.profile_COMMON.image.(@id == imageId).init_from;
				}
				else
					filename = image["init_from"];
	
				if (filename.substr(0, 2) == "./")
				{
					filename = filename.substr( 2 );
				}
			}
			return filename;
		}
		
		/**
		 * Retrieves the color of a material
		 */
		private function parseColorMaterial(colorName:String, materialData:MaterialData):void
		{
			var material:XML = collada["library_materials"].material.(@id == colorName)[0];
			
			if (material) {
				var effectId:String = getId( material["instance_effect"].@url );
				var effect:XML = collada["library_effects"].effect.(@id == effectId)[0];
				
				materialData.ambientColor = getColorValue(effect..ambient[0]);
				materialData.diffuseColor = getColorValue(effect..diffuse[0]);
				materialData.specularColor = getColorValue(effect..specular[0]);
				materialData.shininess = Number(effect..shininess.float[0]);
			}
		}
		
		private function getColorValue(colorXML:XML):uint
		{
			if (!colorXML || colorXML.length() == 0)
				return 0xFFFFFF;
			
			if(!colorXML["color"] || colorXML["color"].length() == 0)
				return 0xFFFFFF;
			
			var colorArray:Array = colorXML["color"].split(" ");
			if(colorArray.length <= 0)
				return 0xFFFFFF;
			
			return int(colorArray[0]*255 << 16) | int(colorArray[1]*255 << 8) | int(colorArray[2]*255);
		}
		
		/**
		 * Converts a data string to an array of objects. Handles vertex and uv objects
		 */
        private function deserialize(input:XML, geo:XML, VObject:Class, output:Array):Array
        {
            var id:String = input.@source.split("#")[1];

            // Source?
            var acc:XMLList = geo..source.(@id == id)["technique_common"].accessor;

            if (acc != new XMLList())
            {
                // Build source floats array
                var floId:String  = acc.@source.split("#")[1];
                var floXML:XMLList = collada..float_array.(@id == floId);
                var floStr:String  = floXML.toString();
                var floats:Array   = getArray(floStr);
    			var float:Number;
                // Build params array
                var params:Array = [];
				var param:String;
				
                for each (var par:XML in acc["param"])
                    params.push(par.@name);

                // Build output array
    			var len:int = floats.length;
    			var i:int = 0;
                while (i < len)
                {
    				var element:ValueObject = new VObject();
	            	if (element is Vertex) {
	            		var vertex:Vertex = element as Vertex;
	                    for each (param in params) {
	                    	float = floats[i];
	                    	switch (param) {
	                    		case VALUE_X:
	                    			if (yUp)
	                    				vertex._x = -float*scaling;
	                    			else
	                    				vertex._x = float*scaling;
	                    			break;
	                    		case VALUE_Y:
	                    				vertex._y = float*scaling;
	                    			break;
	                    			break;
	                    		case VALUE_Z:
	                    				vertex._z = float*scaling;
	                    			break;
	                    			break;
	                    		default:
	                    	}
	                    	++i;
	                    }
		            } else if (element is UV) {
		            	var uv:UV = element as UV;
	                    for each (param in params) {
	                    	float = floats[i];
	                    	switch (param) {
	                    		case VALUE_U:
	                    			uv._u = float;
	                    			break;
	                    		case VALUE_V:
	                    			uv._v = float;
	                    			break;
	                    		default:
	                    	}
	                    	++i;
	                    }
		            }
	                output.push(element);
	            }
            }
            else
            {
                // Store indexes if no source
                var recursive :XMLList = geo..vertices.(@id == id)["input"];

                output = deserialize(recursive[0], geo, VObject, output);
            }

            return output;
        }
    }
}