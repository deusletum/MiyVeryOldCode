using System;
using System.Globalization;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MS.CommandLine
{
    ///	<summary>
    ///	   Collection of options.
    ///	</summary>
    [Serializable]
    public class CommandOptionCollection : NameObjectCollectionBase
    {
        ///	<summary>
        ///	   Default constructor for CommandOptionCollection.
        ///	</summary>
        public CommandOptionCollection()
        {
        }

        ///	<summary>
        ///	   Constructor used	by serialization.
        ///	</summary>
        ///	<param name="info">
        ///	   Serialization info.
        ///	</param>
        ///	<param name="context">
        ///	   Streaming context.
        ///	</param>
        protected CommandOptionCollection(
            SerializationInfo info,	StreamingContext context) :
            base(info, context)
        {
        }

        ///	<summary>
        ///	   Adds	an option to the collection.
        ///	</summary>
        ///	<param name="o">
        ///	   Option to add.
        ///	</param>
        public void	Add(CommandOption option)
        {
            if (option == null)
            {
                throw new ArgumentNullException("option");
            }
            BaseAdd(option.Name.ToLower(CultureInfo.CurrentUICulture), option) ;
        }



        ///	<summary>
        ///	   Removes the option from the collection.
        ///	</summary>
        ///	<param name="option">
        ///	   Option to remove.
        ///	</param>
        public void	Remove(CommandOption option)
        {
            if(option == null)
            {
                throw new ArgumentNullException("option");
            }
            Remove(option.Name);
        }

        ///	<summary>
        ///	   Removes the option with the given name from the collection.
        ///	</summary>
        ///	<param name="optionName">
        ///	   Name	of option.
        ///	</param>
        public void	Remove(string optionName)
        {
            if(optionName == null)
            {
                throw new ArgumentNullException("optionName");
            }

            this.BaseRemove(optionName.ToLower(CultureInfo.CurrentUICulture));
        }

        ///	<summary>
        ///	   Tests whether collection	contains option.
        ///	</summary>
        ///	<param name="name">
        ///	   Option name.
        ///	</param>
        ///	<returns>
        ///	   True	if the collection contains the option.
        ///	</returns>
        public bool	Contains(string	name)
        {
            if (name ==	null)
            {
                throw new ArgumentNullException("name");
            }
            return this.BaseGet(name.ToLower(CultureInfo.CurrentUICulture))!=null ;
        }

        ///	<summary>
        ///	   Indexer gets	the	option of the specified	name.
        ///	</summary>
        public CommandOption this[string name]
        {
            get
            {
                if (name ==	null)
                {
                    throw new ArgumentNullException("name");
                }
                return (CommandOption)BaseGet(name.ToLower(CultureInfo.CurrentUICulture)) ;
            }
        }

        ///	<summary>
        ///	   Copies option collection	to an array.
        ///	</summary>
        ///	<param name="array">
        ///	   Array to	copy to.
        ///	</param>
        ///	<param name="index">
        ///	   Start index.
        ///	</param>
        public void	CopyTo(CommandOption[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        ///	<summary>
        ///	   Insert option into collection.
        ///	</summary>
        ///	<param name="index">
        ///	   Index in	collection.
        ///	</param>
        ///	<param name="option">
        ///	   Option to insert.
        ///	</param>
        public void	Insert(int index, CommandOption	option)	
        {
            ((IList)this).Insert(index,	(object) option);
        }

        ///	<summary>
        ///	   Gets	the	index of the option.
        ///	</summary>
        ///	<param name="option">
        ///	   Option to search	for.
        ///	</param>
        ///	<returns>
        ///	   Index of	option.
        ///	</returns>
        public int IndexOf(CommandOption option) 
        {
            return ((IList)this).IndexOf((object) option);
        }

    }
}
