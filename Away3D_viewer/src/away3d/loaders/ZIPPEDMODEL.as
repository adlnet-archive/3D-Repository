// ActionScript file

package away3d.loaders
{
	import away3d.arcane;
	import away3d.containers.*;
	import away3d.core.base.*;
	import away3d.core.utils.*;
	import away3d.events.*;
	import away3d.loaders.data.*;
	import away3d.loaders.utils.*;
	import away3d.materials.*;
	
	import cmodule.zaail.CLibInit;
	
	import flash.display.*;
	import flash.display.BitmapData;
	import flash.events.*;
	import flash.geom.Rectangle;
	import flash.net.FileReference;
	import flash.utils.*;
	
	import mx.events.FlexEvent;
	
	import nochump.util.zip.*;
	import nochump.util.zip.ZipFile;
	
	use namespace arcane;
	
	/** 
	 * File loader for the zipped models in the ADL 3DR
	 */
	public class ZIPPEDMODEL extends AbstractParser
	{
		protected var lib:Object;
		protected var zloader:CLibInit;
		
		public function ZIPPEDMODEL(init:Object = null)
		{
			super(init);
			
			binary = true;
			zloader = new CLibInit();
			lib = zloader.init();

		}
		
		public static function parse(data:*, init:Object = null):ObjectContainer3D
		{
			return Loader3D.parse(data, ZIPPEDMODEL, init).handle as ObjectContainer3D;
		}
		
		
		public static function load(url:String, init:Object = null):Loader3D
		{
			return Loader3D.load(url, ZIPPEDMODEL, init);
		}
		/** @private */
		arcane override function prepareData(data:*):void
		{
			kmz = Cast.bytearray(data);
			
			kmzFile = new ZipFile(kmz);
			

			for(var k:Number = 0; k < kmzFile.entries.length; ++k) {
				var entry:ZipEntry = kmzFile.entries[k];
				
				if((entry.name.indexOf(".DAE")>-1 || entry.name.indexOf(".dae")>-1)) {
					var entrydata:ByteArray = kmzFile.getInput(entry);
					collada = new XML(entrydata.toString());
					//TODO: swap this to parseGeometry()
					_container = Collada.parse(collada, {autoLoadTextures:false,centerMeshes:true});
					if (container is Loader3D) {
						(container as Loader3D).parser.container.materialLibrary.loadRequired = false;
						(container as Loader3D).addOnSuccess(onParseGeometry);
					} else {
						parseImages();
					}
				}
			}
		}
		/** @private */
		arcane override function parseNext():void
		{
			notifySuccess();
		}
		
		private var kmz:ByteArray;
		private var collada:XML;
		private var kmzFile:ZipFile;
		
		private function onParseGeometry(event:Loader3DEvent):void
		{
			_container = event.loader.handle;
			parseImages();
		}
		
		private function parseImages():void
		{
			_materialLibrary = _container.materialLibrary;
			_materialLibrary.loadRequired = false;
			
			for(var i:int = 0; i < kmzFile.entries.length; ++i) {
				var entry:ZipEntry = kmzFile.entries[i];
				
				var entryname:String = entry.name;
				var find = entryname.lastIndexOf('/');
				entryname = entryname.slice(find+1,entryname.length);
				
				if(entry.name.indexOf(".jpg")>-1 || entry.name.indexOf(".png")>-1) {
					var data:ByteArray = kmzFile.getInput(entry);
					var _loader:Loader = new Loader();
					_loader.name = entryname;
					_loader.contentLoaderInfo.addEventListener(Event.COMPLETE, loadBitmapCompleteHandler);
					
					_loader.loadBytes(data);
				}
				if(entry.name.indexOf(".tga")>-1 || entry.name.indexOf(".dds")>-1)
				{
					var data2:ByteArray = kmzFile.getInput(entry);
					ApplyBitmap(GetBitmapFromData(data2,entryname),entryname);
					
				}
			}
		}
		private function loadBitmapCompleteHandler(e:Event):void {
			var loader:Loader = Loader(e.target["loader"]);
			
			//pass material instance to correct materialData
			var _materialData:MaterialData;
			var _face:Face;
			for each (var tempee in _materialLibrary) {
				if(tempee != false)
				{
					_materialData = tempee;
					if(_materialData.textureFileName != null)
					{
						
						var texturename:String = _materialData.textureFileName;
						var find = texturename.lastIndexOf('/');
						texturename = texturename.slice(find+1,texturename.length);
						
						if (texturename.toLocaleLowerCase()== loader.name.toLocaleLowerCase()) {
							_materialData.textureBitmap = Bitmap(loader.content).bitmapData;
							_materialData.material = new BitmapMaterial(_materialData.textureBitmap,{precision:2,smooth:true});
							for each(_face in _materialData.elements)
							_face.material = _materialData.material as Material;
						}
					}
				}
			}
		}
		private function ApplyBitmap(e:Bitmap,filename:String):void {
			
			
			//pass material instance to correct materialData
			var _materialData:MaterialData;
			var _face:Face;
			for each (var temp in _materialLibrary) {
				if(temp != false)
				{
					_materialData = temp;
					if(_materialData.textureFileName != null)
					{
						var texturename:String = _materialData.textureFileName;
						var find = texturename.lastIndexOf('/');
						texturename = texturename.slice(find+1,texturename.length);
						
						if (texturename.toLocaleLowerCase() == filename.toLocaleLowerCase()) {
	
							_materialData.textureBitmap = e.bitmapData;
							_materialData.material = new BitmapMaterial(_materialData.textureBitmap,{precision:2,smooth:true});
							for each(_face in _materialData.elements)
							_face.material = _materialData.material as Material;
						}
					}
				}
			}
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
		
		private function loadBitmapErrorHandler(e:Event):void {
			var loader:Loader = Loader(e.target["loader"]);
			
			//pass material instance to correct materialData
			var _materialData:MaterialData;
			var _face:Face;
			for each (var temp in _materialLibrary) {
				if(temp != false)
				{
					_materialData = temp;
					if (_materialData.textureFileName == loader.name) {
						_materialData.textureBitmap = Bitmap(loader.content).bitmapData;
						_materialData.material = new WireColorMaterial();
						for each(_face in _materialData.elements)
						_face.material = _materialData.material as Material;
					}
				}
			}
		}
		
		public var containerData:ContainerData;
		
	

		

	}
}
