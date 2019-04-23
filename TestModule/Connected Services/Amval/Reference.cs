﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     //
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Amval
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GoodsCoding", Namespace="http://schemas.datacontract.org/2004/07/Shoniz.WMS.WcfService")]
    public partial class GoodsCoding : object
    {
        
        private string DiagramCodeField;
        
        private string DiagramNameField;
        
        private string GoodsFamilyCodeField;
        
        private string GoodsFamilyNameField;
        
        private string GoodsGroupCodeField;
        
        private string GoodsGroupNameField;
        
        private string GoodsSpecificationsCodeField;
        
        private string GoodsSpecificationsNameField;
        
        private string GoodsSubGroupCodeField;
        
        private string GoodsSubGroupNameField;
        
        private string ProductCodeField;
        
        private string ProductNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DiagramCode
        {
            get
            {
                return this.DiagramCodeField;
            }
            set
            {
                this.DiagramCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DiagramName
        {
            get
            {
                return this.DiagramNameField;
            }
            set
            {
                this.DiagramNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsFamilyCode
        {
            get
            {
                return this.GoodsFamilyCodeField;
            }
            set
            {
                this.GoodsFamilyCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsFamilyName
        {
            get
            {
                return this.GoodsFamilyNameField;
            }
            set
            {
                this.GoodsFamilyNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsGroupCode
        {
            get
            {
                return this.GoodsGroupCodeField;
            }
            set
            {
                this.GoodsGroupCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsGroupName
        {
            get
            {
                return this.GoodsGroupNameField;
            }
            set
            {
                this.GoodsGroupNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsSpecificationsCode
        {
            get
            {
                return this.GoodsSpecificationsCodeField;
            }
            set
            {
                this.GoodsSpecificationsCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsSpecificationsName
        {
            get
            {
                return this.GoodsSpecificationsNameField;
            }
            set
            {
                this.GoodsSpecificationsNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsSubGroupCode
        {
            get
            {
                return this.GoodsSubGroupCodeField;
            }
            set
            {
                this.GoodsSubGroupCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GoodsSubGroupName
        {
            get
            {
                return this.GoodsSubGroupNameField;
            }
            set
            {
                this.GoodsSubGroupNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProductCode
        {
            get
            {
                return this.ProductCodeField;
            }
            set
            {
                this.ProductCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProductName
        {
            get
            {
                return this.ProductNameField;
            }
            set
            {
                this.ProductNameField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Amval.IWmsService")]
    public interface IWmsService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWmsService/GetGoodsCodingById", ReplyAction="http://tempuri.org/IWmsService/GetGoodsCodingByIdResponse")]
        System.Threading.Tasks.Task<Amval.GoodsCoding> GetGoodsCodingByIdAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWmsService/GetGoodsCodingList", ReplyAction="http://tempuri.org/IWmsService/GetGoodsCodingListResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<Amval.GoodsCoding>> GetGoodsCodingListAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface IWmsServiceChannel : Amval.IWmsService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class WmsServiceClient : System.ServiceModel.ClientBase<Amval.IWmsService>, Amval.IWmsService
    {
        
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WmsServiceClient() : 
                base(WmsServiceClient.GetDefaultBinding(), WmsServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IWmsService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WmsServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(WmsServiceClient.GetBindingForEndpoint(endpointConfiguration), WmsServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WmsServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WmsServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WmsServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WmsServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WmsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<Amval.GoodsCoding> GetGoodsCodingByIdAsync(int value)
        {
            return base.Channel.GetGoodsCodingByIdAsync(value);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<Amval.GoodsCoding>> GetGoodsCodingListAsync()
        {
            return base.Channel.GetGoodsCodingListAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IWmsService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IWmsService))
            {
                return new System.ServiceModel.EndpointAddress("http://shonizit:180/WmsService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return WmsServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IWmsService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return WmsServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IWmsService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IWmsService,
        }
    }
}
