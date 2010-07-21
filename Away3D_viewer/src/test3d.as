package {
	
	import away3d.animators.*;
	import away3d.arcane;
	import away3d.cameras.Camera3D;
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
		private var groundPlane:Plane
		
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
		public function CreateGUI()
		{
			
			URLLabel = new TextField();
			URLLabel.text = "URL";
			URLLabel.height = 20;
			addChild(URLLabel);
			
			StatusLabel = new TextField();
			StatusLabel.text = "Paused";
			StatusLabel.height = 20;
			addChild(StatusLabel);
			
			StatusLabel.y = URLLabel.height;
			
			RadiusLabel = new TextField();
			RadiusLabel.text = "URL";
			RadiusLabel.height = 20;
			addChild(RadiusLabel);
			RadiusLabel.y = URLLabel.height + StatusLabel.height;
			
			switchAxis = new Button("Flip Axis");
			
			addChild(switchAxis);
			switchAxis.x = 30;
			switchAxis.y = 0;
			switchAxis.addEventListener(MouseEvent.CLICK,switchaxis);
			
			var uploadFile:Button = new Button("Change Texture");
			
			addChild(uploadFile);
			uploadFile.x = 30 + switchAxis.width;
			uploadFile.width = 220;
			uploadFile.y = 0;
			uploadFile.addEventListener(MouseEvent.CLICK,uploadFileFunc);
			
			slider = new Slider(stage);
					
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
			
			// Initialise Papervision3D
			//init3D();
			
			// Create the 3D objects
			// Create an object container to group the objects on the scene
			group = new ObjectContainer3D();
			scene.addChild(group);
			group.pushback = true;
			group.ownCanvas = true;
			group.screenZOffset = -1000;
			
			var gridMaterial:BitmapMaterial = new  TransformBitmapMaterial(Cast.bitmap(gridBitmap), {smooth:true, precision:2,repeat:true, scaleX:.1, scaleY:.1});;
			//"C:/test3ddata/aak-47.dae"
			loader = ZIPPEDMODEL.load(fileURL,{autoLoadTextures:false, centerMeshes:true});
			loader.addOnSuccess(createScene);
			
			
			// Initialise frame-enter loop
			this.addEventListener(Event.ENTER_FRAME, loop);
			
			stage.addEventListener(MouseEvent.MOUSE_UP, onMouseUp);
			stage.addEventListener(MouseEvent.MOUSE_DOWN, onMouseDown);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, onMouseMove);
			stage.addEventListener(MouseEvent.MOUSE_WHEEL, onMouseScroll);
			
			view.x = stage.stageWidth / 2;
			view.y = stage.stageHeight / 2;
			
			//URLLabel.text = fileURL;
			//StatusLabel.text = "Loading";
			
			
			
		}

		public function test3d() {
			
			
			ExternalInterface.addCallback("Load",Load);
			mCenter = new Number3D(0,0,0);
			
			zloader = new CLibInit();
			lib = zloader.init();
			
			init3D();
			//Load("C:/test3ddata/su27.zip");
			CreateGUI();
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
		private function switchaxis(e:Event)
		{
			var temp = yvec;
			yvec = zvec;
			zvec = temp;
			
			if(groundPlane.rotationX == 0)
			{
				groundPlane.rotationX = -90;
				groundPlane.y = fb.GetCenter().y;
				groundPlane.x = fb.GetCenter().x;
				groundPlane.z = fb.GetMin().z;
				
				temp = mOffsetVec.y;
				mOffsetVec.y = mOffsetVec.z;
				mOffsetVec.z = temp;
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
			}
			updateCamVec();
		}
		private function init3D():void {
			
			// Create a new scene where all the 3D object will be rendered
			scene = new Scene3D();
			 
			// Create a new camera, passing some initialisation parameters
			camera = new Camera3D({zoom:2000, focus:.3, x:0, y:0, z:0});
			
			
			
			
			camera.lookAt(new Number3D(0,0,0));
			
			
			// Create a new view that encapsulates the scene and the camera
			view = new View3D({scene:scene, camera:camera});
			
			// center the viewport to the middle of the stage
			view.x = stage.stageWidth / 2;
			view.y = stage.stageHeight / 2;
			addChild(view);
			
		}
		private function onMouseScroll(Event:MouseEvent):void {
			
			if(Event.delta > 0)
				for(var i:Number = 0; i < Math.abs(Event.delta); i++)
					zoom *= 1.1;
			if(Event.delta < 0)
				for(var i:Number = 0; i < Math.abs(Event.delta); i++)
					zoom *= 0.9;

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

		private function createScene(event:Event):void {
			
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
			rad.sub(fb.GetCenter(),fb.GetMin());
			mRadius = rad.distance(new Number3D(0,0,0));
			
			//mCenter = loader.handle.center;
			mOffsetVec.normalize(mRadius * 2);
			
			RadiusLabel.text = String(mRadius);
			zoom = mRadius * 2;
			
			var gridMaterial:BitmapMaterial = new  TransformBitmapMaterial(Cast.bitmap(gridBitmap), {smooth:true, precision:2,repeat:true, scaleX:.1, scaleY:.1});;
			
			groundPlane = new Plane({material:gridMaterial, segmentsW:10, segmentsH:10});
			group.addChild((groundPlane));
			groundPlane.width = mRadius*10;
			groundPlane.height = mRadius*10;
			groundPlane.ownCanvas = true;
			groundPlane.screenZOffset = 10000;
			groundPlane.y = fb.GetMin().y;
			groundPlane.x = fb.GetCenter().x;
			groundPlane.z = fb.GetCenter().z;
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
