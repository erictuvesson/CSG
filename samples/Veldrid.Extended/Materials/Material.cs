namespace Veldrid.Materials
{
    public abstract class Material
    {
        /// <summary>
        /// Applies all the shader parameters before drawing any primitives.
        /// </summary>
        public void BeginApply(DrawingContext context)
        {
            OnBeginApply(context, context.PreviousMaterial);
        }

        /// <summary>
        /// Restores any shader parameters changes after drawing the primitives.
        /// </summary>
        public void EndApply(DrawingContext context)
        {
            OnEndApply(context);
            context.PreviousMaterial = this;
        }

        /// <summary>
        /// Applies all the shader parameters before drawing any primitives.
        /// </summary>
        protected abstract void OnBeginApply(DrawingContext context, Material previousMaterial);

        /// <summary>
        /// Applies all the shader parameters before drawing any primitives.
        /// </summary>
        protected abstract void OnEndApply(DrawingContext context);
    }
}
