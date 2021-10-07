using System;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;

namespace MS.CommandLine
{

    /// <summary>
    ///    A collection of option specs.
    /// </summary>
    public class OptionSpecificationCollection : CollectionBase
    {
        /// <summary>
        ///    Adds the specification to the collection.
        /// </summary>
        /// <param name="specification">
        ///    Specification.
        /// </param>
        public void Add(OptionSpecification specification)
        {
            if (specification == null)
            {
                throw new OptionSpecificationException("Specification passed to add is null");
            }

            if(Contains(specification.OptionName))
            {
                throw new OptionSpecificationException(string.Format(CultureInfo.CurrentCulture, "Option {0} is specified twice", specification.OptionName));
            }
            this.InnerList.Add(specification);
        }

        /// <summary>
        ///    Returns true if collection contains option.
        /// </summary>
        /// <param name="optionName">
        ///    Name of the option.
        /// </param>
        /// <returns>
        ///    True if the option is in the collection.
        /// </returns>
        public bool Contains( string optionName )
        {
            foreach( OptionSpecification spec in InnerList )
            {
                if (string.Compare(spec.OptionName, optionName, true, CultureInfo.CurrentCulture) == 0)
                {
                    return true ;
                }
            }
            return false ;
        }

        /// <summary>
        ///    Used in debug code to assert that two collections
        ///    have disjoint set of options (no overlap)
        /// </summary>
        /// <param name="otherCollection">
        ///    Other collection to compare with.
        /// </param>
        /// <returns>
        ///    True if both collections are disjoint.  False if
        ///    there is overlap.
        /// </returns>
        public bool IsDisjoint(OptionSpecificationCollection otherCollection)
        {
            if(otherCollection == null)
            {
                throw new OptionSpecificationException("Collection specified is null");
            }

            foreach (OptionSpecification spec in InnerList)
            {
                if (otherCollection.Contains(spec.OptionName))
                {
                    return false ;
                }
            }
            return true ;
        }

        /// <summary>
        ///    Gets an option spec from the collection.
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
        public OptionSpecification this[string optionName]
        {
            get
            {
                foreach (OptionSpecification spec in InnerList)
                {
                    if (string.Compare(spec.OptionName, optionName, true, CultureInfo.CurrentUICulture) == 0)
                    {
                        return spec ;
                    }
                }
                return null ;
            }
        }

        /// <summary>
        ///    Gets or sets the specification at the specified index.
        /// </summary>
        public OptionSpecification this[int index]
        {
            get 
            {
                return (OptionSpecification) ((IList)this)[index];
            }
            set 
            {

                ((IList)this)[index] =  value;
            }
        }

        /// <summary>
        ///    Copies the specifications to an array.
        /// </summary>
        /// <param name="array">
        ///    Destination array.
        /// </param>
        /// <param name="index">
        ///    Index in array.
        /// </param>
        public void CopyTo(OptionSpecification[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        /// <summary>
        ///    Inserts the specification into the collection.
        /// </summary>
        /// <param name="index">
        ///    Index to insert at.
        /// </param>
        /// <param name="specification">
        ///    Specification to insert.
        /// </param>
        public void Insert(int index, OptionSpecification specification) 
        {
            ((IList)this).Insert(index, (object) specification);
        }

        /// <summary>
        ///    Removes the specification from the collection.
        /// </summary>
        /// <param name="specification">
        ///    Specification to remove.
        /// </param>
        public void Remove(OptionSpecification specification) 
        {
            ((IList)this).Remove((object) specification);
        }

        /// <summary>
        ///    Gets the index of the specificiation in the array.
        /// </summary>
        /// <param name="specification">
        ///    Specification.
        /// </param>
        /// <returns>
        ///    Index of specification.
        /// </returns>
        public int IndexOf(OptionSpecification specification) 
        {
            return ((IList)this).IndexOf((object) specification);
        }


        /// <summary>
        ///    This gets a specification based on a partial option name.
        /// </summary>
        /// <param name="partialOptionName">
        ///    Partial option name.
        /// </param>
        /// <returns>
        ///    Returns the option specification that starts with the option name.
        /// </returns>
        public OptionSpecification[] GetPartial(string partialOptionName)
        {
            if (partialOptionName == null)
            {
                throw new OptionSpecificationException("Option name cannot be null");
            }

            ArrayList returnSpecs = new ArrayList();
            foreach(OptionSpecification spec in this.InnerList)
            {
                if(string.Compare(partialOptionName, 0, spec.OptionName, 0, partialOptionName.Length, true, CultureInfo.CurrentCulture) == 0)
                {
                    returnSpecs.Add(spec);
                }
            }
            return (OptionSpecification[])returnSpecs.ToArray(typeof(OptionSpecification));
        }

        /// <summary>
        ///    Loads the option collection from the specified type.
        /// </summary>
        /// <param name="t">
        ///    Type containing specification attributes.
        /// </param>
        public void LoadFromType(Type type)
        {
            if (type == null)
            {
                throw new OptionSpecificationException("Type passed to LoadFromType is null");
            }

            //
            // Load options set on the class.
            //
            object[] optionAttributes = type.GetCustomAttributes(typeof(OptionAttribute), true);
            foreach (OptionAttribute attribute in optionAttributes)
            {
                OptionSpecification spec = new OptionSpecification(attribute, null);
                Add(spec);
            }
            //
            // Load options on any properties.
            //
            foreach(PropertyInfo propertyInfo in type.GetProperties())
            {
                object[] propOptionAttributes = propertyInfo.GetCustomAttributes(typeof(OptionAttribute), true);
                if(propOptionAttributes.Length == 1)
                {
                    if(propertyInfo.GetSetMethod() == null && !typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        throw new OptionSpecificationException(
                            string.Format(CultureInfo.CurrentCulture, "The property '{0}' has no setter so it cannot be used as an option.", propertyInfo.Name));
                    }
                    OptionAttribute attribute = propOptionAttributes[0] as OptionAttribute;
                    Debug.Assert(attribute != null, "attribute != null");
                    OptionSpecification spec = new OptionSpecification(attribute, propertyInfo);
                    Add(spec);
                }
                else if(propOptionAttributes.Length == 0)
                {
                    // this has not been marked with a property so skip over it.
                }
                else
                {
                    throw new OptionSpecificationException(
                        string.Format(CultureInfo.CurrentCulture, "The property '{0}' has been marked with two or more options", propertyInfo.Name));
                }
            }
        }
    }
}
