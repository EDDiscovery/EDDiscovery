using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.UserControls
{
    // https://stackoverflow.com/questions/6817107/abstract-usercontrol-inheritance-in-visual-studio-designer#answer-17661386
    public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider() : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
            {
                return typeof(TBase);
            }
            else
            {
                return base.GetReflectionType(objectType, instance);
            }
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
            {
                objectType = typeof(TBase);
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
