﻿namespace CSG.Viewer.Framework.Materials
{
    using Veldrid;

    public abstract class Material
    {
        protected DrawingContext DrawingContext { get; }

        protected Material(DrawingContext context)
        {
            this.DrawingContext = context;
        }

        /// <summary>
        /// Applies all the shader parameters before drawing any primitives.
        /// </summary>
        public void BeginApply(CommandList commandList, DrawingContext context)
        {
            OnBeginApply(commandList, context, context.PreviousMaterial);
        }

        /// <summary>
        /// Restores any shader parameters changes after drawing the primitives.
        /// </summary>
        public void EndApply(CommandList commandList, DrawingContext context)
        {
            OnEndApply(commandList, context);
            context.PreviousMaterial = this;
        }

        /// <summary>
        /// Applies all the shader parameters before drawing any primitives.
        /// </summary>
        protected abstract void OnBeginApply(CommandList commandList, DrawingContext context, Material previousMaterial);

        /// <summary>
        /// Applies all the shader parameters before drawing any primitives.
        /// </summary>
        protected abstract void OnEndApply(CommandList commandList, DrawingContext context);
    }
}
