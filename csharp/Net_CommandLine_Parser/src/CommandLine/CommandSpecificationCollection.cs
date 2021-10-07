using System;
using System.Globalization;
using System.Collections;

namespace MS.CommandLine
{
	/// <summary>
	///    A collection of command specifications.
	/// </summary>
	public class CommandSpecificationCollection : CollectionBase
	{
		public CommandSpecificationCollection()
		{
		}

        /// <summary>
        ///    Allows access to a specific specification by the name or alias of the command.
        /// </summary>
        public CommandSpecification this[string name]
        {
            get
            {
                foreach(CommandSpecification cs in this.InnerList)
                {
                    if(string.Compare(name , cs.Name, true, CultureInfo.CurrentCulture) == 0)
                    {
                        return cs;
                    }
                    else if (cs.CommandAliases.Count > 0)
                    {
                        // The command has aliases.  Need to check those too.
                        //
                        foreach(string alias in cs.CommandAliases)
                        {
                            if(string.Compare(name, alias, true, CultureInfo.CurrentCulture) == 0)
                            {
                                return cs;
                            }
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        ///    Gets the command specification at the specified index.
        /// </summary>
        public CommandSpecification this[int index]
        {
            get
            {
                return this.InnerList[index] as CommandSpecification;
            }
        }

        /// <summary>
        ///    Copy the collection to an array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(CommandSpecification[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        /// <summary>
        ///    Add a new specification.
        /// </summary>
        /// <param name="specification">
        ///    Specification to add.
        /// </param>
        /// <returns>
        ///    Index in collection.
        /// </returns>
        public int Add(CommandSpecification specification)
        {
            return ((IList)this).Add ((object) specification);
        }

        /// <summary>
        ///    Gets whether the specification is in the collection.
        /// </summary>
        /// <param name="specification">
        ///    Specification
        /// </param>
        /// <returns>
        ///    True if the specification is in the collection.
        /// </returns>
        public bool Contains(CommandSpecification specification) 
        {
            return ((IList)this).Contains((object) specification);
        }

        /// <summary>
        ///    Inserts a specification at the specified index.
        /// </summary>
        /// <param name="index">
        ///    Index to insert specification.
        /// </param>
        /// <param name="specification">
        ///    Specification to insert.
        /// </param>
        public void Insert(int index, CommandSpecification specification) 
        {
            ((IList)this).Insert(index, (object) specification);
        }

        /// <summary>
        ///    Remove the specified command specification.
        /// </summary>
        /// <param name="specification">
        ///    Specification to remove.
        /// </param>
        public void Remove(CommandSpecification specification) 
        {
            ((IList)this).Remove((object) specification);
        }

        /// <summary>
        ///    Gets the index of the specification.
        /// </summary>
        /// <param name="specification">
        ///    Specification to search for.
        /// </param>
        /// <returns>
        ///    Index of specification.
        /// </returns>
        public int IndexOf(CommandSpecification specification) 
        {
            return ((IList)this).IndexOf((object) specification);
        }
	}
}
