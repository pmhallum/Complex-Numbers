using System;
using System.Globalization;

namespace ComplexNum
{
    
    public class Complex
    {

        /*
         * Constructors
         * */
        #region Constructors
        /// <summary>
        /// Creates instance of a complex number
        /// </summary>
        /// <param name="r">Real part</param>
        /// <param name="i">Imaginary part</param>
        public Complex(double r, double i)
        {
            this.Real = r;
            this.Imag = i;
        }

        /// <summary>
        /// Creates instance of a complex number
        /// </summary>
        public Complex()
        {
            this.Real = 0;
            this.Imag = 0;
        }
        #endregion


        /*
             * Properties
             * */
        #region Properties

        /// <summary>
        /// Get or set the real part of the complex number
        /// </summary>  
        public Double Real { get; set; }


        /// <summary>
        /// Get or set the imaginary part of the complex number
        /// </summary>  
        public Double Imag { get; set; }


        /// <summary>
        /// Get or set the absolute value
        /// </summary>        
        public Double Abs
        {
            get
            {
                return Math.Sqrt(this.Real * this.Real + this.Imag * this.Imag);
            }
            set
            {
                this.Real = Math.Cos(this.Angle) * value;
                this.Imag = Math.Sin(this.Angle) * value;
            }
        }

        /// <summary>
        /// Get or set the angle in radians
        /// </summary>  
        public Double Angle
        {
            get
            {
                return Math.Atan2(this.Imag, this.Real);
            }
            set
            {
                this.Real = Math.Cos(value) * this.Abs;
                this.Imag = Math.Sin(value) * this.Abs;
            }
        }

        #endregion


        /*
             * Operators
             * */
        #region Operators
        public static Complex operator +(Complex c1, Complex c2)
        {
            Complex ret = new Complex();
            ret.Real = c1.Real + c2.Real;
            ret.Imag = c1.Imag + c2.Imag;
            return ret;
        }
        public static Complex operator -(Complex c1, Complex c2)
        {
            Complex ret = new Complex();
            ret.Real = c1.Real - c2.Real;
            ret.Imag = c1.Imag - c2.Imag;
            return ret;
        }
        public static Complex operator *(Complex c1, Complex c2)
        {
            Complex ret = new Complex();
            ret.Real = c1.Real * c2.Real - c1.Imag * c2.Imag;
            ret.Imag = c1.Imag * c2.Real + c1.Real * c2.Imag;
            return ret;
        }
        public static Complex operator *(Complex c, double d)
        {
            Complex ret = new Complex();
            ret.Real = c.Real * d;
            ret.Imag = c.Imag * d;
            return ret;
        }

        public static Complex operator *(double d, Complex c)
        {
            return c * d;
        }

        public static Complex operator /(Complex c1, Complex c2)
        {
            Complex ret = new Complex();
            ret.Real = (c1.Real * c2.Real + c1.Imag * c2.Imag) / (c2.Real * c2.Real + c2.Imag * c2.Imag);
            ret.Imag = (c1.Imag * c2.Real - c1.Real * c2.Imag) / (c2.Real * c2.Real + c2.Imag * c2.Imag);
            return ret;

        }
        public static Complex operator /(Complex c, double d)
        {
            Complex ret = new Complex();
            ret.Real = c.Real / d;
            ret.Imag = c.Imag / d;
            return ret;
        }
        public static Complex operator /(double d, Complex c)
        {
            return (new Complex(d, 0)) / c;
        }
        #endregion


        /*
             * Methods
             * */
        #region Methods

        /// <summary>
        /// Converts the complex number to its string representation.
        /// </summary>
        /// <param name="provider">Culture format</param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider)
        {
            String sign = "+";
            if (this.Imag < 0)
                sign = "";
            return this.Real.ToString(provider) + sign + this.Imag.ToString(provider) + "i";
        }

        /// <summary>
        /// Converts the complex number to its string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Returns the complex number normalized to unity
        /// </summary>  
        public static Complex Norm(Complex complex)
        {
            return new Complex(complex.Real / complex.Abs, complex.Imag / complex.Abs);
            /*complex.Real = complex.Real / complex.Abs;
            complex.Imag = complex.Imag / complex.Abs;
            return complex;*/
        }
        /// <summary>
        /// Returns the complex number normalized to unity
        /// </summary>  
        public Complex Norm()
        {
            return Norm(this);
        }

        /// <summary>
        /// Returns the complex conjugate
        /// </summary>  
        public static Complex Conj(Complex complex)
        {
            return new Complex(complex.Real, -complex.Imag);
        }
        /// <summary>
        /// Returns the complex conjugate
        /// </summary>  
        public Complex Conj()
        {
            return Complex.Conj(this);
        }

        /// <summary>
        /// Linearly interpolates two complex numbers on a z-plane by the fraction p, p=0 returns c1 and p=1 returns c2.  Both angle and amplitude are interpolated.
        /// This gives smooth transformation from one complex to the other
        /// </summary>
        /// <param name="c1">First complex in boundary</param>
        /// <param name="c2">Second complex in boundary</param>
        /// <param name="p">Fraction of interpolation, must be on the closed interval of 0 to 1</param>
        static public Complex LinearInterpolation(Complex c1, Complex c2, Double p)
        {
            if (p < 0 || p > 1)
                throw new Exception("p must be between on the closed interval 0 to 1");
            Complex ret;
            if (Complex.IsZero(c1))
            {
                ret = c2 * p;
            }
            else
                if (Complex.IsZero(c2))
                {
                    ret = c1 * (1 - p);
                }
                else
                {
                    Double Angle = c1.Angle + Complex.Dot(c1, c2).Angle * p;
                    Double Abs = c1.Abs + (c2.Abs - c1.Abs) * p;
                    ret = new Complex(Abs * Math.Cos(Angle), Abs * Math.Sin(Angle));
                }
            return new Complex(ret.Real, ret.Imag);
        }




        /// <summary>
        /// Returns a value indicating whether the specified complex number evaluates to not a number
        /// </summary>  
        /// <param name="c">Complex number to evaluate</param>
        public static bool IsNaN(Complex c)
        {
            return (Double.IsNaN(c.Real) || Double.IsNaN(c.Imag));
        }
        /// <summary>
        /// Returns a value indicating whether the specified number evaluates to not a number
        /// </summary>    
        public bool IsNaN()
        {
            return Complex.IsNaN(this);
        }


        /// <summary>
        /// Returns a value indicating whether the specified complex number evaluates to zero.
        /// </summary>
        /// <param name="c">Complex number to evaluate</param>
        /// <returns></returns>
        public static Boolean IsZero(Complex c)
        {
            return (c.Real == 0 && c.Imag == 0);
        }
        /// <summary>
        /// Returns a value indicating whether the specified complex number evaluates to zero.
        /// </summary>         
        public bool IsZero()
        {
            return Complex.IsZero(this);

        }

        /// <summary>
        /// Returns the complex dot product
        /// </summary>  
        public static Complex Dot(Complex c1, Complex c2)
        {
            return c1.Conj() * c2;
        }
        /// <summary>
        /// Returns the complex dot product
        /// </summary>  
        public Complex Dot(Complex complex)
        {
            return Complex.Dot(this, complex);
        }


        /// <summary>
        /// Rounds the real and imaginary parts of the complex number
        /// </summary>
        /// <param name="complex"></param>
        /// <param name="decimals">Number of fractional digits</param>
        /// <returns></returns>
        public static Complex Round(Complex complex, Int32 decimals)
        {
            return new Complex(Math.Round(complex.Real, decimals), Math.Round(complex.Imag, decimals));
        }
        /// <summary>
        /// Rounds the real and imaginary parts of the complex number
        /// </summary>
        /// <param name="complex"></param>
        /// <param name="decimals">Number of fractional digits</param>
        public Complex Round(Int32 decimals)
        {
            return Complex.Round(this, decimals);
        }


        /// <summary>
        /// Returns a complex number with given angle of unit length
        /// </summary>  
        /// <param name="c">Angle of the complex number in radians</param>        
        public static Complex FromAngle(double rad)
        {
            return new Complex(Math.Cos(rad), Math.Sin(rad));
        }


        /// <summary>
        /// Tries to parse string to a complex number.
        /// I or j can be used for complex term, e.g. '3+5i' or '3+5j'
        /// </summary>
        /// <param name="str">String to parse</param>
        /// <returns>Complex number</returns>
        public static Complex FromString(String str)
        {

            str = str.Replace(",", ".");
            String[] strs = str.Split(new String[] { " ", "+", "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length > 2 || strs.Length <= 0)
                throw new Exception("Could not convert " + str + " to complex");

            Complex ret = new Complex(0, 0);
            foreach (String num in strs)
            {
                Int32 index = num.ToUpper().IndexOfAny(new char[] { 'i', 'I', 'j', 'J' });
                if (index >= 0)
                    ret.Imag = Convert.ToDouble(num.Substring(0, index), CultureInfo.InvariantCulture);
                else
                    ret.Real = Convert.ToDouble(num, CultureInfo.InvariantCulture);

            }
            return ret;
        }


        /// <summary>
        /// Converts array of complex to array of doubles.  Each complex number occupies two doubles where the first holds the real part and the second hold the complex part.
        /// </summary>
        /// <param name="array">Doubles to convert</param>
        /// <returns></returns>
        public static Complex[] FromDouble(Double[] array)
        {
            int remainder = 0;
            int quotient = Math.DivRem(array.Length, 2, out remainder);
            if (remainder != 0)
                throw new Exception("Length of input array needs to be a multiple of two.");

            Complex[] c = new Complex[quotient];
            for (int k = 0; k < c.Length; k++)
            {
                c[k] = new Complex(array[2 * k], array[2 * k + 1]);
            }
            return c;
        }


        /// <summary>
        /// Converts array of complex to array of doubles.  Each complex number occupies two doubles where the first holds the real part and the second hold the complex part.
        /// </summary>
        /// <param name="complex">Complex numbers to convert</param>
        /// <returns></returns>
        public static Double[] ToDouble(Complex[] complex)
        {
            Double[] array = new Double[complex.Length * 2];
            for (int k = 0; k < complex.Length; k++)
            {
                array[k * 2] = complex[k].Real;
                array[k * 2 + 1] = complex[k].Imag;
            }
            return array;
        }

        #endregion
    }


}
