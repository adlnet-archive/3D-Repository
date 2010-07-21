package
{
	import away3d.containers.ObjectContainer3D;
	
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.DisplayObject;
	import flash.display.DisplayObjectContainer;
	import flash.display.MovieClip;
	import flash.events.*;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	public class Slider extends MovieClip
	{
		[Embed(source="/./assets/chit.png")] private var ChitImage:Class;
		private var chitImage:Bitmap = new ChitImage();
		[Embed(source="/./assets/track.png")] private var TrackImage:Class;
		private var trackImage:Bitmap = new TrackImage();
		private var mMouseDown:Boolean = false;
		private var object:ObjectContainer3D;
		private var scale:Number;
		public function setObject(inobject:ObjectContainer3D)
		{
			scale = inobject.scaleX;
			object = inobject;
		}
		public function Slider(parent:DisplayObjectContainer)
		{
			parent.addChild(this);
			super();
			
			addChild(trackImage);
			addChild(chitImage);
			chitImage.x = 380;
			trackImage.x = 280;
			this.addEventListener(MouseEvent.MOUSE_UP, onMouseUp);
			this.addEventListener(MouseEvent.MOUSE_DOWN, onMouseDown);
			this.parent.addEventListener(MouseEvent.MOUSE_MOVE, onMouseMove);
			this.addEventListener(MouseEvent.MOUSE_WHEEL, onMouseScroll);
			this.addEventListener(MouseEvent.MOUSE_UP, onMouseUp);
			this.parent.addEventListener(MouseEvent.MOUSE_UP, onMouseUp);
		}
		private function onMouseScroll(Event:MouseEvent):void {
			
			
		}
		private function onMouseMove(Event:MouseEvent):void {
			
			if(mMouseDown)
			{
				chitImage.x = Event.stageX;
				if(object)
				{
					var scale2 = chitImage.x - 380;
					
					scale2/=10;
					object.scaleX = scale + scale2;
					object.scaleY = scale + scale2;
					object.scaleZ = scale + scale2;
				}
			}
			
		}
		private function onMouseDown(Event:MouseEvent):void {
			
			mMouseDown = true;
			
		}
		private function onMouseUp(Event:MouseEvent):void {
			
			mMouseDown = false;	
		} 
	}
}