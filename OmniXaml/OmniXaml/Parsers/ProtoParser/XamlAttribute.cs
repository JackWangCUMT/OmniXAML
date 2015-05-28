namespace OmniXaml.Parsers.ProtoParser
{
    using Typing;

    internal class XamlAttribute
    {
        public XamlAttribute(UnboundAttribute unboundAttribute, XamlType type, IXamlTypeRepository typeContext)
        {
            Type = unboundAttribute.Type;
            Value = unboundAttribute.Value;
            Locator = unboundAttribute.Locator;

            Property = GetProperty(Locator, type, typeContext);
        }

        public string Value { get; private set; }

        public AttributeType Type { get; private set; }

        public PropertyLocator Locator { get; }

        public XamlMember Property { get; private set; }

        private XamlMember GetProperty(PropertyLocator propLocator, XamlType xamType, IXamlTypeRepository typingCore)
        {
            return propLocator.IsDotted ? GetAttachableMember(propLocator, typingCore) : GetRegularMember(xamType, typingCore);
        }

        private XamlMember GetRegularMember(XamlType tagType, IXamlTypeRepository typeRepository)
        {
            return typeRepository.Get(tagType.UnderlyingType).GetMember(Locator.PropertyName);
        }

        private XamlMember GetAttachableMember(PropertyLocator memberLocator, IXamlTypeRepository typeRepository)
        {
            var owner = memberLocator.OwnerName;
            var ownerType = typeRepository.GetByPrefix(memberLocator.Prefix, owner);
            return typeRepository.Get(ownerType.UnderlyingType).GetAttachableMember(Locator.PropertyName);
        }
    }
}