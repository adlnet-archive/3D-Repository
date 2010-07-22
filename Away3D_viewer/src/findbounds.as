package  
{
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
	import away3d.loaders.*;
	import away3d.loaders.data.*;
	import away3d.loaders.utils.*;
	import away3d.materials.*;
	import away3d.materials.ColorMaterial;
	import away3d.materials.TransformBitmapMaterial;
	import away3d.primitives.*;
	
	class findbounds
	{
		private var verticies:Array;
		private var max:Number3D;
		private var min:Number3D;
		private var center:Number3D;
		public function findbounds()
		{
			verticies = new Array();	
			min = new Number3D;
			max = new Number3D;
			
			max.x = -Infinity;
			max.y = -Infinity;
			max.z = -Infinity;
			
			min.x = Infinity;
			min.y = Infinity;
			min.z = Infinity;
		}
		public function apply2(node:Mesh):void
		{
			var parentcoords:MatrixAway3D = node.sceneTransform;
				var geom:Geometry = node.geometry;
				for(var j = 0; j < geom.vertices.length; j++)
				{
					var vert:Number3D = new Number3D;
					vert.x = geom.vertices[j].x;
					vert.y = geom.vertices[j].y;
					vert.z = geom.vertices[j].z;
					
					vert.transform(vert,parentcoords);
					verticies.push(vert);
				}
				
			
		}
		public function GetCenter():Number3D
		{
			return center;
		}
		public function GetMin():Number3D
		{
			return min;
		}
		public function GetMax():Number3D
		{
			return max;
		}
		public function traverse_internal(node:Object)
		{
			
			if(node is ObjectContainer3D)
			{
				for( var i = 0; i < (node as ObjectContainer3D).children.length; i++)
				{
					traverse((node as ObjectContainer3D).children[i]);
				}
			}
			else if (node is Mesh)
			{
				apply2(node as Mesh);
			}
		}
		public function traverse(node:Object)
		{
			traverse_internal(node);
			
			var total:Number3D = new Number3D;
			for(var i =0; i < verticies.length;i++)
			{
				total.add(total,verticies[i]);
				if(verticies[i].x > max.x)
					max.x = verticies[i].x;
				if(verticies[i].y > max.y)
					max.y = verticies[i].y;
				if(verticies[i].z > max.z)
					max.z = verticies[i].z;
				
				if(verticies[i].x < min.x)
					min.x = verticies[i].x;
				if(verticies[i].y < min.y)
					min.y = verticies[i].y;
				if(verticies[i].z < min.z)
					min.z = verticies[i].z;
			}
			total.x /= verticies.length;
			total.y /= verticies.length;
			total.z /= verticies.length;
			center = total;
		}
	}
}