﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClienteGloomApp.ServicioGloom {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioGloom.IServicioAdministrador", CallbackContract=typeof(ClienteGloomApp.ServicioGloom.IServicioAdministradorCallback))]
    public interface IServicioAdministrador {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServicioAdministrador/Sum")]
        void Sum(int a, int b);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServicioAdministrador/Sum")]
        System.Threading.Tasks.Task SumAsync(int a, int b);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioAdministradorCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioAdministrador/Response", ReplyAction="http://tempuri.org/IServicioAdministrador/ResponseResponse")]
        void Response(int result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioAdministradorChannel : ClienteGloomApp.ServicioGloom.IServicioAdministrador, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioAdministradorClient : System.ServiceModel.DuplexClientBase<ClienteGloomApp.ServicioGloom.IServicioAdministrador>, ClienteGloomApp.ServicioGloom.IServicioAdministrador {
        
        public ServicioAdministradorClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServicioAdministradorClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServicioAdministradorClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioAdministradorClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioAdministradorClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Sum(int a, int b) {
            base.Channel.Sum(a, b);
        }
        
        public System.Threading.Tasks.Task SumAsync(int a, int b) {
            return base.Channel.SumAsync(a, b);
        }
    }
}