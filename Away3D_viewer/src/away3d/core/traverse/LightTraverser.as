package away3d.core.traverse
{
	import away3d.containers.ObjectContainer3D;
	import away3d.core.base.*;
	import away3d.core.light.*;
    

    /**
    * Traverser that gathers blocker primitives for occlusion culling.
    */
    public class LightTraverser extends Traverser
    { 	
		/**
		 * Creates a new <code>LightTraverser</code> object.
		 */
        public function LightTraverser()
        {
        }
        
		/**
		 * @inheritDoc
		 */
		public override function match(node:Object3D):Boolean
        {
			return node.visible;
        }
        
		/**
		 * @inheritDoc
		 */
        public override function apply(node:Object3D):void
        {
            //clear light arrays
            if (node.ownLights)
            	node.lightarray.clear();
            
            if (node is ObjectContainer3D) {
            	var container:ObjectContainer3D = node as ObjectContainer3D;
            	var lightProvider:ILightProvider;
            	for each (lightProvider in container.lights) {
            		lightProvider.light(container.lightarray);
            	}
            }
        }

    }
}
