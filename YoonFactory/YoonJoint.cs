using System.Xml.Serialization;

namespace YoonFactory
{
    public class YoonJointD : IYoonJoint, IYoonJoint<double>
    {
        public IYoonJoint Clone()
        {
            YoonJointD j = new YoonJointD();
            j.J1 = J1;
            j.J2 = J2;
            j.J3 = J3;
            j.J4 = J4;
            j.J5 = J5;
            j.J6 = J6;
            return j;
        }

        public void CopyFrom(IYoonJoint j)
        {
            if (j is YoonJointD joint)
            {
                J1 = joint.J1;
                J2 = joint.J2;
                J3 = joint.J3;
                J4 = joint.J4;
                J5 = joint.J5;
                J6 = joint.J6;
            }
        }

        [XmlIgnore]
        public double J1 { get; set; }
        [XmlAttribute]
        public double J2 { get; set; }
        [XmlAttribute]
        public double J3 { get; set; }
        [XmlIgnore]
        public double J4 { get; set; }
        [XmlAttribute]
        public double J5 { get; set; }
        [XmlAttribute]
        public double J6 { get; set; }

        public double[] ToArray
        {
            get => new double[6] { J1, J2, J3, J4, J5, J6 };
        }

        public YoonJointD()
        {
            Zero();
        }
        public YoonJointD(IYoonJoint j)
        {
            CopyFrom(j);
        }
        public YoonJointD(double dJ1, double dJ2, double dJ3, double dJ4, double dJ5, double dJ6)
        {
            J1 = dJ1;
            J2 = dJ2;
            J3 = dJ3;
            J4 = dJ4;
            J5 = dJ5;
            J6 = dJ6;
        }

        public void Zero()
        {
            J1 = J2 = J3 = J4 = J5 = J6 = 0;
        }
        public static YoonJointD operator *(YoonJointD j, double a)
        {
            return new YoonJointD(j.J1 * a, j.J2 * a, j.J3 * a, j.J4 * a, j.J5 * a, j.J6 * a);
        }
        public static YoonJointD operator +(YoonJointD j1, YoonJointD j2)
        {
            return new YoonJointD(j1.J1 + j2.J1, j1.J2 + j2.J2, j1.J3 + j2.J3, j1.J4 + j2.J4, j1.J5 + j2.J5, j1.J6 + j2.J6);
        }
        public static YoonJointD operator -(YoonJointD j1, YoonJointD j2)
        {
            return new YoonJointD(j1.J1 - j2.J1, j1.J2 - j2.J2, j1.J3 - j2.J3, j1.J4 - j2.J4, j1.J5 - j2.J5, j1.J6 - j2.J6);
        }
        public static YoonJointD operator /(YoonJointD j, double a)
        {
            return new YoonJointD(j.J1 / a, j.J2 / a, j.J3 / a, j.J4 / a, j.J5 / a, j.J6 / a);
        }
    }

}
