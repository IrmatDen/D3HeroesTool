using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3HeroesTool.Utils
{
    // Greatly inspired by Jon Skeet's post: http://stackoverflow.com/a/255630

    /// <summary>
    /// Doesn't feel as right as templated interface, but makes it usable from reflection code
    /// </summary>
    public interface IBiMap
    {
        // We don't really need LeftKeys nor RightKeys since they're both Values of each other
        IEnumerable LeftValues { get; }
        IEnumerable RightValues { get; }

        void Add(object left, object right);
        bool TryGetByLeft(object leftKey, out object rightValue);
        bool TryGetByRight(object rightKey, out object leftValue);
    }

    public class SimpleBiMap<LeftType, RightType> : IBiMap
    {
        IDictionary<LeftType, RightType> leftToRight = new Dictionary<LeftType, RightType>();
        IDictionary<RightType, LeftType> rightToLeft = new Dictionary<RightType, LeftType>();

        public IEnumerable LeftValues { get { return leftToRight.Keys; } }
        public IEnumerable RightValues { get { return rightToLeft.Keys; } }

        public int Count { get { return leftToRight.Count; } }

        public void Add(LeftType left, RightType right)
        {
            if (leftToRight.ContainsKey(left))
                throw new ArgumentException(String.Format("{0} already registered as left key", left.ToString()));
            if (rightToLeft.ContainsKey(right))
                throw new ArgumentException(String.Format("{0} already registered as right key", right.ToString()));

            leftToRight.Add(left, right);
            try
            {
                rightToLeft.Add(right, left);
            }
            catch (Exception ex)
            {
                try
                {
                    leftToRight.Remove(left);
                }
                catch (Exception nestedEx)
                {
                    throw new Exception("Couldn't cleanup after failure from reverse dictionary. Structure corrupted",
                        nestedEx);
                }
                throw ex;
            }
        }

        public bool TryGetByLeft(LeftType k, out RightType v)
        {
            return leftToRight.TryGetValue(k, out v);
        }

        public bool TryGetByRight(RightType k, out LeftType v)
        {
            return rightToLeft.TryGetValue(k, out v);
        }

        void IBiMap.Add(object left, object right)
        {
            Add((LeftType)left, (RightType)right);
        }

        bool IBiMap.TryGetByLeft(object leftKey, out object rightValue)
        {
            RightType wanted;
            bool res = TryGetByLeft((LeftType)leftKey, out wanted);
            rightValue = wanted;
            return res;
        }

        bool IBiMap.TryGetByRight(object rightKey, out object leftValue)
        {
            LeftType wanted;
            bool res = TryGetByRight((RightType)rightKey, out wanted);
            leftValue = wanted;
            return res;
        }
    }
}
