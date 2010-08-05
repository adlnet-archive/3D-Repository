package {
	
	import away3d.animators.*;
	import away3d.arcane;
	import away3d.cameras.Camera3D;
	import away3d.cameras.lenses.PerspectiveLens;
	import away3d.containers.*;
	import away3d.core.base.*;
	import away3d.core.math.*;
	import away3d.core.render.BasicRenderer;
	import away3d.core.traverse.PrimitiveTraverser;
	import away3d.core.utils.*;
	import away3d.core.utils.Cast;
	import away3d.core.utils.Color;
	import away3d.events.*;
	import away3d.loaders.*;
	import away3d.loaders.data.*;
	import away3d.loaders.utils.*;
	import away3d.materials.*;
	import away3d.materials.ColorMaterial;
	import away3d.materials.TransformBitmapMaterial;
	import away3d.primitives.*;
	import away3d.test.Button;
	import away3d.test.Slide;
	import away3d.core.clip.*;
	import cmodule.zaail.CLibInit;
	
	import flash.display.*;
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.Sprite;
	import flash.display.StageAlign;
	import flash.display.StageScaleMode;
	import flash.events.*;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;
	import flash.geom.Rectangle;
	import flash.net.FileReference;
	import flash.text.*;
	import flash.utils.*;
	import flash.utils.getDefinitionByName;
	import flash.utils.getQualifiedClassName;
	
	import mx.events.FlexEvent;
	
	import nochump.util.zip.*;
	import nochump.util.zip.ZipFile;
	
	
	[SWF(backgroundColor="#FFFFFF")]
	
	
	public class test3d extends Sprite {
		
		
		[Embed(source="/./assets/grid2.png")] private var GridImage:Class;
		private var gridBitmap:Bitmap = new GridImage();
		
		private static const ORBITAL_RADIUS:Number = 150;
		private var slider:Slider;
		private var scene:Scene3D;
		private var camera:Camera3D;
		private var view:View3D;
		private var zoom:Number;
		private var group:ObjectContainer3D;
		private var sphere:Sphere;
		private var cube:Cube;
		private var centerCube:Cube;
		private var cylinder:Cylinder;
		private var torus:Torus;
		private var mMouseDown:Boolean;
		private var mRelX:Number;
		private var mRelY:Number;
		private var mOldX:Number;
		private var mOldY:Number;
		private var mOffsetVec:Number3D;
		private var mRadius:Number;
		private var groundPlane:Plane;
		private var uploadFile:Button;
		var xvec:Number3D = new Number3D(1,0,0);
		var yvec:Number3D= new Number3D(0,1,0);
		var zvec:Number3D= new Number3D(0,0,1);
		
		private var mCenter;
		
		
		private var URLLabel:TextField;
		private var StatusLabel:TextField;
		private var RadiusLabel:TextField;
		protected var lib:Object;
		protected var zloader:CLibInit;
		private var file:FileReference;
		private var loader:Loader3D;
		private var switchAxis:Button;
		private var gfileURL:String;
		private var mLoadingError:Boolean;
		private var fb:findbounds;
		private var mAxis:String;
		private var kmzFile:ZipFile;
		public function test3d() {
			
			
			ExternalInterface.addCallback("Load",Load);
			ExternalInterface.addCallback("ShowControls",ShowControls);
			ExternalInterface.addCallback("HideControls",HideControls);
			ExternalInterface.addCallback("SetScale",SetScale);
			ExternalInterface.addCallback("SetUpVec",SetUpVec);			
			ExternalInterface.addCallback("MapTexture",MapTexture);		
			
			mCenter = new Number3D(0,0,0);
			mAxis = "Z";
			zloader = new CLibInit();
			lib = zloader.init();
			
			init3D();
			//Load("C:/test3ddata/SU27.zip");
			CreateGUI();
			
		}
		public function init3D():void {
			
			// Create a new scene where all the 3D object will be rendered
			scene = new Scene3D();
			
			// Create a new camera, passing some initialisation parameters
			camera = new Camera3D({zoom:1, focus:45, x:0, y:0, z:0});
			camera.fov = 45;
			var lens:PerspectiveLens = new PerspectiveLens();
			camera.lens = lens;
			
			camera.fixedZoom = true;
			camera.fov = 45;
			camera.zoom = 1;
			
			camera.lookAt(new Number3D(0,0,0));
			
			
			// Create a new view that encapsulates the scene and the camera
			view = new View3D({scene:scene, camera:camera});
			view.clipping = new FrustumClipping({minZ:
				.01}); 
			// center the viewport to the middle of the stage
			view.x = stage.stageWidth / 2;
			view.y = stage.stageHeight / 2;
			addChild(view);
			
		}
		public function CreateGUI()
		{
			
			switchAxis = new Button("Flip Axis");
			
			addChild(switchAxis);
			switchAxis.x = 30;
			switchAxis.y = 0;
			switchAxis.addEventListener(MouseEvent.CLICK,switchaxis);
			
			uploadFile = new Button("Change Texture");
			
			addChild(uploadFile);
			uploadFile.x = 30 + switchAxis.width;
			uploadFile.width = 220;
			uploadFile.y = 0;
			uploadFile.addEventListener(MouseEvent.CLICK,uploadFileFunc);
			
			slider = new Slider(stage);
			
			switchAxis.visible = false;
			uploadFile.visible = false;
			slider.visible = false;
			
			var myVar:RotateButton_ART = new RotateButton_ART();
			addChild(myVar);
			
			myVar.width = 30;
			myVar.height = 30;
			myVar.x = 20;
			myVar.y = 20;
			myVar.addEventListener(MouseEvent.CLICK, switchaxis);
		}
		public function Load(fileURL:String)
		{
			
			xvec = new Number3D(1,0,0);
			yvec= new Number3D(0,0,1);
			zvec= new Number3D(0,1,0);
			
			mLoadingError = false;
			gfileURL = fileURL;
			zoom = 1;
			mRelX = 0;
			mRelY = 0;
			// set up the stage
			stage.align = StageAlign.TOP_LEFT;
			stage.scaleMode = StageScaleMode.NO_SCALE;
			
			mOffsetVec = new Number3D(.5,.5,.5);
			
			// Add resize event listener
			stage.addEventListener(Event.RESIZE, onResize);
			
			// Create the 3D objects
			// Create an object container to group the objects on the scene
			group = new ObjectContainer3D();
			scene.addChild(group);
			group.pushback = true;
			group.ownCanvas = true;
			group.screenZOffset = -1000;
			
			var gridMaterial:BitmapMaterial = new  TransformBitmapMaterial(Cast.bitmap(gridBitmap), {smooth:true, precision:2,repeat:true, scaleX:.1, scaleY:.1});;
			if(fileURL.indexOf("zip",fileURL.length - 3) != -1 || fileURL.indexOf("ZIP",fileURL.length - 3) != -1 || fileURL.indexOf("Zip",fileURL.length - 3) != -1)
				loader = ZIPPEDMODEL.load(fileURL,{autoLoadTextures:false, centerMeshes:true});
			if(fileURL.indexOf("dae",fileURL.length - 3) != -1 || fileURL.indexOf("DAE",fileURL.length - 3) != -1 || fileURL.indexOf("Dae",fileURL.length - 3) != -1)
				loader = Collada.load(fileURL,{autoLoadTextures:false, centerMeshes:true});
			
			loader.addOnSuccess(createScene);
			
			
			
			// Initialise frame-enter loop
			this.addEventListener(Event.ENTER_FRAME, loop);
			
			stage.addEventListener(MouseEvent.MOUSE_UP, onMouseUp);
			stage.addEventListener(MouseEvent.MOUSE_DOWN, onMouseDown);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, onMouseMove);
			stage.addEventListener(MouseEvent.MOUSE_WHEEL, onMouseScroll);
			
			view.x = stage.stageWidth / 2;
			view.y = stage.stageHeight / 2;
			
		}
		private function createScene(event:Event):void {
			
			if(loader.parser is ZIPPEDMODEL)
				kmzFile = (loader.parser as ZIPPEDMODEL).kmzFile;
			
			if(mLoadingError)
				return;
			//StatusLabel.text = "Success";
			//group.addChild(loader.handle);
			
			var group2:ObjectContainer3D = new ObjectContainer3D;
			group2.addChild(loader.handle);
			group.addChild(group2);
			slider.setObject(group2);
			fb = new findbounds();
			fb.traverse(loader.handle);//.traverse(fb);
			mCenter = fb.GetCenter();
			
			loader.handle.pivotPoint = mCenter;
			
			mCenter.sub(mCenter,loader.handle.pivotPoint);
			var rad:Number3D = new Number3D;
			rad.x = fb.GetCenter().x;
			rad.y = fb.GetCenter().y;
			rad.z = fb.GetCenter().z;
			rad.sub(fb.GetMax(),fb.GetMin());
			mRadius = rad.distance(new Number3D(0,0,0))/2;
			
			//mCenter = loader.handle.center;
			mOffsetVec.normalize(mRadius * 2);
			
			zoom = mRadius * 2;
			
			var gridMaterial:BitmapMaterial = new  TransformBitmapMaterial(Cast.bitmap(gridBitmap), {smooth:true, precision:2,repeat:true, scaleX:.1, scaleY:.1});;
			
			groundPlane = new Plane({material:gridMaterial, segmentsW:10, segmentsH:10});
			group.addChild((groundPlane));
			groundPlane.width = mRadius*10;
			groundPlane.height = mRadius*10;
			groundPlane.ownCanvas = true;
			groundPlane.screenZOffset = 10000;
			
			placegrid();
			
			
			
			
			//MapTexture("braces.tga","metal_bars.tga");
		}
		
		public function ShowControls()
		{
			switchAxis.visible = true;
			uploadFile.visible = true;
			slider.visible = true;
		}
		public function HideControls()
		{	
			switchAxis.visible = false;
			uploadFile.visible = false;
			slider.visible = false;
		}
		private function GetBitmapFromData(data:ByteArray, filename:String):Bitmap
		{
			var fileContents:ByteArray = data;
			var output:ByteArray = new ByteArray();
			zloader.supplyFile(filename, fileContents);
			
			lib.ilInit();
			lib.ilOriginFunc(ZaaILInterface.IL_ORIGIN_UPPER_LEFT);
			lib.ilEnable(ZaaILInterface.IL_ORIGIN_SET);
			
			if(lib.ilLoadImage(filename) != 1)    // 1 means successful load
			{
				//Alert.show("Could not load the selected image", "Error Loading Image");
			}
			
			var width:int = lib.ilGetInteger(ZaaILInterface.IL_IMAGE_WIDTH);
			var height:int = lib.ilGetInteger(ZaaILInterface.IL_IMAGE_HEIGHT);
			var depth:int = lib.ilGetInteger(ZaaILInterface.IL_IMAGE_DEPTH);
			lib.ilGetPixels(0, 0, 0, width, height, depth, output);
			output.position = 0;
			
			var bmd:BitmapData = new BitmapData(width, height);
			bmd.setPixels(new Rectangle(0, 0, width, height), output);
			
			var bitmap:Bitmap = new Bitmap(bmd);
			return bitmap;
		}
		private function GetZipFileData(NewTex:String,zip:ZipFile):ByteArray
		{
			for(var i:int = 0; i < zip.entries.length; ++i) {
				var entry:ZipEntry = zip.entries[i];
				
				var entryname:String = entry.name;
				var find = entryname.lastIndexOf('/');
				entryname = entryname.slice(find+1,entryname.length);
				
				if(entryname == NewTex) {
					var data:ByteArray = zip.getInput(entry);
					return data;
				}
			}
			return null;
		}
		private function MapTexture(Orig:String,NewTex:String)
		{
			//MapTextureR(loader.handle,Orig,NewTex);
			var bit:Bitmap = GetBitmapFromData(GetZipFileData(NewTex,kmzFile),NewTex);
			for each( var varr in  loader.handle.materialLibrary)
			{
				
				if(varr != false)
				{
					
					if(varr != null)
					{
						
						var mat:MaterialData = varr;
						
						if( mat.material is BitmapMaterial)
						{
							if((mat.textureFileName == Orig))
							{
								
								(mat.material as BitmapMaterial).bitmap = bit.bitmapData;
							}
						}
					}
				}
				
			}
			
		}
		private function MapTextureR(node:Object3D,Orig:String,NewTex:String)
		{
			if(node is ObjectContainer3D)
			{
				for( var i = 0; i < (node as ObjectContainer3D).children.length; i++)
				{
					MapTextureR((node as ObjectContainer3D).children[i],Orig,NewTex);
				}
				if((node as ObjectContainer3D).material is BitmapMaterial)
				{
					((node as ObjectContainer3D).material as BitmapMaterial).bitmap = bit.bitmapData;	
				}
				if((node as ObjectContainer3D).material is BitmapMaterial)
				{
					((node as ObjectContainer3D).material as BitmapMaterial).bitmap = bit.bitmapData;	
				}
			}
			if (node is Mesh)
			{
				var mesh:Mesh = node as Mesh;
				
				var bit:Bitmap = GetBitmapFromData(GetZipFileData(NewTex,kmzFile),NewTex);
				
				
				for each (var face:Face in mesh.faces)
				{
					if( face.material is BitmapMaterial)
					{
						(face.material as BitmapMaterial).bitmap = bit.bitmapData;
					}
					
				}
				if(mesh.material is BitmapMaterial)
				{
					(mesh.material as BitmapMaterial).bitmap = bit.bitmapData;	
				}
			}
			
		}
		private function swapTexR(node:Object3D)
		{
			if(node is ObjectContainer3D)
			{
				for( var i = 0; i < (node as ObjectContainer3D).children.length; i++)
				{
					swapTexR((node as ObjectContainer3D).children[i]);
				}
			}
			if (node is Mesh)
			{
				var mesh:Mesh = node as Mesh;
				
				var bit:Bitmap = GetBitmapFromData(file.data,file.name);
				
				
				for each (var face:Face in mesh.faces)
				{
					if( face.material is BitmapMaterial)
					{
						(face.material as BitmapMaterial).bitmap = bit.bitmapData;
						
					}
					
				}
			}
			
		}
		private function swapTexture(e:Event)
		{
			swapTexR(loader.handle);
		}
		private function selectFile(e:Event)
		{
			file.load();
		}
		private function uploadFileFunc(e:Event)
		{
			file = new FileReference();
			file.addEventListener(Event.COMPLETE,swapTexture);
			file.addEventListener(Event.SELECT,selectFile);
			
			file.browse();
		}
		private function placegrid()
		{
			var temp;
			if(groundPlane == undefined)
				return;
			
			if(zvec.z == 1)
			{
				groundPlane.rotationX = -90;
				groundPlane.y = fb.GetCenter().y;
				groundPlane.x = fb.GetCenter().x;
				groundPlane.z = fb.GetMin().z;
				
				temp = mOffsetVec.y;
				mOffsetVec.y = mOffsetVec.z;
				mOffsetVec.z = temp;
				mAxis = "Z";
			}		
				
			else
			{
				groundPlane.rotationX = 0;
				groundPlane.y = fb.GetMin().y;
				groundPlane.x = fb.GetCenter().x;
				groundPlane.z = fb.GetCenter().z;
				
				temp = mOffsetVec.y;
				mOffsetVec.y = mOffsetVec.z;
				mOffsetVec.z = temp;
				mAxis = "Y";
			}
			
		}
		private function switchaxis(e:Event)
		{
			var temp = yvec;
			yvec = zvec;
			zvec = temp;
			
			placegrid();
			
			updateCamVec();
		}
		
		private function onMouseScroll(Event:MouseEvent):void {
			
			if(Event.delta > 0)
				for(var i:Number = 0; i < Math.abs(Event.delta); i++)
					zoom *= 0.9;
			if(Event.delta < 0)
				for(var i:Number = 0; i < Math.abs(Event.delta); i++)
					zoom *= 1.1;
			
			mOffsetVec.normalize(zoom);
			
		}
		private function onMouseMove(Event:MouseEvent):void {
			
			
			mRelX = mOldX - (Event.stageX);
			mRelY = mOldY - (Event.stageY);
			
			mOldX = (Event.stageX);
			mOldY = (Event.stageY);
			
		}
		private function onMouseDown(Event:MouseEvent):void {
			
			mMouseDown = true;
			
		}
		private function onMouseUp(Event:MouseEvent):void {
			
			mMouseDown = false;
			
		} 
		private function SetScale(scale:Number):void {
			
			loader.handle.scaleX = scale;
			loader.handle.scaleY = scale;
			loader.handle.scaleZ = scale;
		}
		private function SetUpVec(axis:String):void {
			
			if(axis == "Y" && mAxis == "Z")
			{
				switchaxis(null);
			}
			if(axis == "Z" && mAxis == "Y")
			{
				switchaxis(null);
			}
		} 
		
		private function updateCamVec()
		{
			var temp:Number = mOffsetVec.distance(new Number3D(0,0,0));
			
			
			var xrot:Quaternion = new Quaternion();
			xrot.axis2quaternion(zvec.x,zvec.y,zvec.z,-mRelX/100);
			var matx:away3d.core.math.MatrixAway3D = new MatrixAway3D();
			matx.quaternion2matrix(xrot);
			mOffsetVec.rotate(mOffsetVec,matx);
			
			
			var side:Number3D= new Number3D(0,0,0);
			side.cross(zvec,mOffsetVec);
			side.normalize();
			
			var yrot:Quaternion = new Quaternion();
			yrot.axis2quaternion(side.x,side.y,side.z,-mRelY/100);
			var maty:away3d.core.math.MatrixAway3D = new MatrixAway3D();
			maty.quaternion2matrix(yrot);
			mOffsetVec.rotate(mOffsetVec,maty);
			
			mOffsetVec.normalize(temp);
			
			
		}
		private function loop(event:Event):void {
			
			if(mMouseDown)
			{
				updateCamVec();
				
			}
			
			mRelX = 0;
			mRelY = 0;
			
			camera.x = mCenter.x + mOffsetVec.x;
			camera.y = mCenter.y + mOffsetVec.y;
			camera.z = mCenter.z + mOffsetVec.z;
			
			// Render the 3D scene
			camera.lookAt(mCenter,zvec);
			view.render();
		}
		
		private function onResize(event:Event):void {
			view.x = stage.stageWidth / 2;
			view.y = stage.stageHeight / 2;
		}
	}
}
