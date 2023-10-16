﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//
//     Zmiany w tym pliku mogą spowodować niewłaściwe zachowanie i zostaną utracone
//     w przypadku ponownego wygenerowania kodu.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceReference1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", ConfigurationName="ServiceReference1.CDNOffLineSrvSoap")]
    public interface CDNOffLineSrvSoap
    {
        
        // CODEGEN: Trwa generowanie kontraktu komunikatu, ponieważ nazwa elementu ReceiveDataResult z przestrzeni nazw http://www.comarch.pl/cdn/Products/CDN XL/ nie ma atrybutu nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/ReceiveData", ReplyAction="*")]
        ServiceReference1.ReceiveDataResponse ReceiveData(ServiceReference1.ReceiveDataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/ReceiveData", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.ReceiveDataResponse> ReceiveDataAsync(ServiceReference1.ReceiveDataRequest request);
        
        // CODEGEN: Trwa generowanie kontraktu komunikatu, ponieważ nazwa elementu data z przestrzeni nazw http://www.comarch.pl/cdn/Products/CDN XL/ nie ma atrybutu nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/SendData", ReplyAction="*")]
        ServiceReference1.SendDataResponse SendData(ServiceReference1.SendDataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/SendData", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.SendDataResponse> SendDataAsync(ServiceReference1.SendDataRequest request);
        
        // CODEGEN: Trwa generowanie kontraktu komunikatu, ponieważ nazwa elementu sqlCommand z przestrzeni nazw http://www.comarch.pl/cdn/Products/CDN XL/ nie ma atrybutu nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/ExecuteSQLCommand", ReplyAction="*")]
        ServiceReference1.ExecuteSQLCommandResponse ExecuteSQLCommand(ServiceReference1.ExecuteSQLCommandRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/ExecuteSQLCommand", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.ExecuteSQLCommandResponse> ExecuteSQLCommandAsync(ServiceReference1.ExecuteSQLCommandRequest request);
        
        // CODEGEN: Trwa generowanie kontraktu komunikatu, ponieważ nazwa elementu sqlCommand z przestrzeni nazw http://www.comarch.pl/cdn/Products/CDN XL/ nie ma atrybutu nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/ExecuteSQLXmlCommand", ReplyAction="*")]
        ServiceReference1.ExecuteSQLXmlCommandResponse ExecuteSQLXmlCommand(ServiceReference1.ExecuteSQLXmlCommandRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.comarch.pl/cdn/Products/CDN XL/ExecuteSQLXmlCommand", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.ExecuteSQLXmlCommandResponse> ExecuteSQLXmlCommandAsync(ServiceReference1.ExecuteSQLXmlCommandRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ReceiveDataRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ReceiveData", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.ReceiveDataRequestBody Body;
        
        public ReceiveDataRequest()
        {
        }
        
        public ReceiveDataRequest(ServiceReference1.ReceiveDataRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/")]
    public partial class ReceiveDataRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int oddzialID;
        
        public ReceiveDataRequestBody()
        {
        }
        
        public ReceiveDataRequestBody(int oddzialID)
        {
            this.oddzialID = oddzialID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ReceiveDataResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ReceiveDataResponse", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.ReceiveDataResponseBody Body;
        
        public ReceiveDataResponse()
        {
        }
        
        public ReceiveDataResponse(ServiceReference1.ReceiveDataResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/")]
    public partial class ReceiveDataResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public byte[] ReceiveDataResult;
        
        public ReceiveDataResponseBody()
        {
        }
        
        public ReceiveDataResponseBody(byte[] ReceiveDataResult)
        {
            this.ReceiveDataResult = ReceiveDataResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendDataRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendData", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.SendDataRequestBody Body;
        
        public SendDataRequest()
        {
        }
        
        public SendDataRequest(ServiceReference1.SendDataRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/")]
    public partial class SendDataRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public byte[] data;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int oddzialID;
        
        public SendDataRequestBody()
        {
        }
        
        public SendDataRequestBody(byte[] data, int oddzialID)
        {
            this.data = data;
            this.oddzialID = oddzialID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendDataResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendDataResponse", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.SendDataResponseBody Body;
        
        public SendDataResponse()
        {
        }
        
        public SendDataResponse(ServiceReference1.SendDataResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class SendDataResponseBody
    {
        
        public SendDataResponseBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ExecuteSQLCommandRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ExecuteSQLCommand", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.ExecuteSQLCommandRequestBody Body;
        
        public ExecuteSQLCommandRequest()
        {
        }
        
        public ExecuteSQLCommandRequest(ServiceReference1.ExecuteSQLCommandRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/")]
    public partial class ExecuteSQLCommandRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string sqlCommand;
        
        public ExecuteSQLCommandRequestBody()
        {
        }
        
        public ExecuteSQLCommandRequestBody(string sqlCommand)
        {
            this.sqlCommand = sqlCommand;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ExecuteSQLCommandResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ExecuteSQLCommandResponse", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.ExecuteSQLCommandResponseBody Body;
        
        public ExecuteSQLCommandResponse()
        {
        }
        
        public ExecuteSQLCommandResponse(ServiceReference1.ExecuteSQLCommandResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/")]
    public partial class ExecuteSQLCommandResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ExecuteSQLCommandResult;
        
        public ExecuteSQLCommandResponseBody()
        {
        }
        
        public ExecuteSQLCommandResponseBody(string ExecuteSQLCommandResult)
        {
            this.ExecuteSQLCommandResult = ExecuteSQLCommandResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ExecuteSQLXmlCommandRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ExecuteSQLXmlCommand", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.ExecuteSQLXmlCommandRequestBody Body;
        
        public ExecuteSQLXmlCommandRequest()
        {
        }
        
        public ExecuteSQLXmlCommandRequest(ServiceReference1.ExecuteSQLXmlCommandRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/")]
    public partial class ExecuteSQLXmlCommandRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string sqlCommand;
        
        public ExecuteSQLXmlCommandRequestBody()
        {
        }
        
        public ExecuteSQLXmlCommandRequestBody(string sqlCommand)
        {
            this.sqlCommand = sqlCommand;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ExecuteSQLXmlCommandResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ExecuteSQLXmlCommandResponse", Namespace="http://www.comarch.pl/cdn/Products/CDN XL/", Order=0)]
        public ServiceReference1.ExecuteSQLXmlCommandResponseBody Body;
        
        public ExecuteSQLXmlCommandResponse()
        {
        }
        
        public ExecuteSQLXmlCommandResponse(ServiceReference1.ExecuteSQLXmlCommandResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.comarch.pl/cdn/Products/CDN XL/")]
    public partial class ExecuteSQLXmlCommandResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public byte[] ExecuteSQLXmlCommandResult;
        
        public ExecuteSQLXmlCommandResponseBody()
        {
        }
        
        public ExecuteSQLXmlCommandResponseBody(byte[] ExecuteSQLXmlCommandResult)
        {
            this.ExecuteSQLXmlCommandResult = ExecuteSQLXmlCommandResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface CDNOffLineSrvSoapChannel : ServiceReference1.CDNOffLineSrvSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class CDNOffLineSrvSoapClient : System.ServiceModel.ClientBase<ServiceReference1.CDNOffLineSrvSoap>, ServiceReference1.CDNOffLineSrvSoap
    {
        
        /// <summary>
        /// Wdróż tę metodę częściową, aby skonfigurować punkt końcowy usługi.
        /// </summary>
        /// <param name="serviceEndpoint">Punkt końcowy do skonfigurowania</param>
        /// <param name="clientCredentials">Poświadczenia klienta</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public CDNOffLineSrvSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(CDNOffLineSrvSoapClient.GetBindingForEndpoint(endpointConfiguration), CDNOffLineSrvSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CDNOffLineSrvSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(CDNOffLineSrvSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CDNOffLineSrvSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(CDNOffLineSrvSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CDNOffLineSrvSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference1.ReceiveDataResponse ServiceReference1.CDNOffLineSrvSoap.ReceiveData(ServiceReference1.ReceiveDataRequest request)
        {
            return base.Channel.ReceiveData(request);
        }
        
        public byte[] ReceiveData(int oddzialID)
        {
            ServiceReference1.ReceiveDataRequest inValue = new ServiceReference1.ReceiveDataRequest();
            inValue.Body = new ServiceReference1.ReceiveDataRequestBody();
            inValue.Body.oddzialID = oddzialID;
            ServiceReference1.ReceiveDataResponse retVal = ((ServiceReference1.CDNOffLineSrvSoap)(this)).ReceiveData(inValue);
            return retVal.Body.ReceiveDataResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.ReceiveDataResponse> ServiceReference1.CDNOffLineSrvSoap.ReceiveDataAsync(ServiceReference1.ReceiveDataRequest request)
        {
            return base.Channel.ReceiveDataAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.ReceiveDataResponse> ReceiveDataAsync(int oddzialID)
        {
            ServiceReference1.ReceiveDataRequest inValue = new ServiceReference1.ReceiveDataRequest();
            inValue.Body = new ServiceReference1.ReceiveDataRequestBody();
            inValue.Body.oddzialID = oddzialID;
            return ((ServiceReference1.CDNOffLineSrvSoap)(this)).ReceiveDataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference1.SendDataResponse ServiceReference1.CDNOffLineSrvSoap.SendData(ServiceReference1.SendDataRequest request)
        {
            return base.Channel.SendData(request);
        }
        
        public void SendData(byte[] data, int oddzialID)
        {
            ServiceReference1.SendDataRequest inValue = new ServiceReference1.SendDataRequest();
            inValue.Body = new ServiceReference1.SendDataRequestBody();
            inValue.Body.data = data;
            inValue.Body.oddzialID = oddzialID;
            ServiceReference1.SendDataResponse retVal = ((ServiceReference1.CDNOffLineSrvSoap)(this)).SendData(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.SendDataResponse> ServiceReference1.CDNOffLineSrvSoap.SendDataAsync(ServiceReference1.SendDataRequest request)
        {
            return base.Channel.SendDataAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.SendDataResponse> SendDataAsync(byte[] data, int oddzialID)
        {
            ServiceReference1.SendDataRequest inValue = new ServiceReference1.SendDataRequest();
            inValue.Body = new ServiceReference1.SendDataRequestBody();
            inValue.Body.data = data;
            inValue.Body.oddzialID = oddzialID;
            return ((ServiceReference1.CDNOffLineSrvSoap)(this)).SendDataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference1.ExecuteSQLCommandResponse ServiceReference1.CDNOffLineSrvSoap.ExecuteSQLCommand(ServiceReference1.ExecuteSQLCommandRequest request)
        {
            return base.Channel.ExecuteSQLCommand(request);
        }
        
        public string ExecuteSQLCommand(string sqlCommand)
        {
            ServiceReference1.ExecuteSQLCommandRequest inValue = new ServiceReference1.ExecuteSQLCommandRequest();
            inValue.Body = new ServiceReference1.ExecuteSQLCommandRequestBody();
            inValue.Body.sqlCommand = sqlCommand;
            ServiceReference1.ExecuteSQLCommandResponse retVal = ((ServiceReference1.CDNOffLineSrvSoap)(this)).ExecuteSQLCommand(inValue);
            return retVal.Body.ExecuteSQLCommandResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.ExecuteSQLCommandResponse> ServiceReference1.CDNOffLineSrvSoap.ExecuteSQLCommandAsync(ServiceReference1.ExecuteSQLCommandRequest request)
        {
            return base.Channel.ExecuteSQLCommandAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.ExecuteSQLCommandResponse> ExecuteSQLCommandAsync(string sqlCommand)
        {
            ServiceReference1.ExecuteSQLCommandRequest inValue = new ServiceReference1.ExecuteSQLCommandRequest();
            inValue.Body = new ServiceReference1.ExecuteSQLCommandRequestBody();
            inValue.Body.sqlCommand = sqlCommand;
            return ((ServiceReference1.CDNOffLineSrvSoap)(this)).ExecuteSQLCommandAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiceReference1.ExecuteSQLXmlCommandResponse ServiceReference1.CDNOffLineSrvSoap.ExecuteSQLXmlCommand(ServiceReference1.ExecuteSQLXmlCommandRequest request)
        {
            return base.Channel.ExecuteSQLXmlCommand(request);
        }
        
        public byte[] ExecuteSQLXmlCommand(string sqlCommand)
        {
            ServiceReference1.ExecuteSQLXmlCommandRequest inValue = new ServiceReference1.ExecuteSQLXmlCommandRequest();
            inValue.Body = new ServiceReference1.ExecuteSQLXmlCommandRequestBody();
            inValue.Body.sqlCommand = sqlCommand;
            ServiceReference1.ExecuteSQLXmlCommandResponse retVal = ((ServiceReference1.CDNOffLineSrvSoap)(this)).ExecuteSQLXmlCommand(inValue);
            return retVal.Body.ExecuteSQLXmlCommandResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.ExecuteSQLXmlCommandResponse> ServiceReference1.CDNOffLineSrvSoap.ExecuteSQLXmlCommandAsync(ServiceReference1.ExecuteSQLXmlCommandRequest request)
        {
            return base.Channel.ExecuteSQLXmlCommandAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.ExecuteSQLXmlCommandResponse> ExecuteSQLXmlCommandAsync(string sqlCommand)
        {
            ServiceReference1.ExecuteSQLXmlCommandRequest inValue = new ServiceReference1.ExecuteSQLXmlCommandRequest();
            inValue.Body = new ServiceReference1.ExecuteSQLXmlCommandRequestBody();
            inValue.Body.sqlCommand = sqlCommand;
            return ((ServiceReference1.CDNOffLineSrvSoap)(this)).ExecuteSQLXmlCommandAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.CDNOffLineSrvSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.CDNOffLineSrvSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Nie można znaleźć punktu końcowego o nazwie „{0}”.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.CDNOffLineSrvSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://serwer.szachownica.com.pl/cdnofflinesrv/cdnofflinesrv.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.CDNOffLineSrvSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://serwer.szachownica.com.pl/cdnofflinesrv/cdnofflinesrv.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Nie można znaleźć punktu końcowego o nazwie „{0}”.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            CDNOffLineSrvSoap,
            
            CDNOffLineSrvSoap12,
        }
    }
}