//java code at https://github.com/googlecodelabs/tensorflow-for-poets-2


using RectF = Android.Graphics.RectF;
namespace co.elrashid.xam.tf.Incp.and.Tensorflow
{
    public class Classifier_Recognition : Java.Lang.Object
    {
         
        internal readonly string id;

        
        internal readonly string title;

        
        internal readonly float? confidence;

        
        internal RectF location;

       
        public Classifier_Recognition(string id, string title, float? confidence, RectF location)
        {
            this.id = id;
            this.title = title;
            this.confidence = confidence;
            this.location = location;
        }

        public virtual string Id
        {
            get
            {
                return id;
            }
        }

        public virtual string Title
        {
            get
            {
                return title;
            }
        }

        public virtual float? Confidence
        {
            get
            {
                return confidence;
            }
        }

        public virtual RectF Location
        {
            get
            {
                return new RectF(location);
            }
            set
            {
                this.location = value;
            }
        }


        public override string ToString()
        {
            string resultString = "";
            if (!string.ReferenceEquals(id, null))
            {
                resultString += "[" + id + "] ";
            }

            if (!string.ReferenceEquals(title, null))
            {
                resultString += title + " ";
            }

            if (confidence != null)
            {
                resultString += string.Format("({0:F1}%) ", confidence * 100.0f);
            }

            if (location != null)
            {
                resultString += location + " ";
            }

            return resultString.Trim();
        }
    }
}