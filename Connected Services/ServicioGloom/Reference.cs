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
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Jugador", Namespace="http://schemas.datacontract.org/2004/07/BlbibliotecaClases")]
    [System.SerializableAttribute()]
    public partial class Jugador : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string apellidosField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string contraseñaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string correoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string iconoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nombreUsuarioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string tipoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string apellidos {
            get {
                return this.apellidosField;
            }
            set {
                if ((object.ReferenceEquals(this.apellidosField, value) != true)) {
                    this.apellidosField = value;
                    this.RaisePropertyChanged("apellidos");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string contraseña {
            get {
                return this.contraseñaField;
            }
            set {
                if ((object.ReferenceEquals(this.contraseñaField, value) != true)) {
                    this.contraseñaField = value;
                    this.RaisePropertyChanged("contraseña");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string correo {
            get {
                return this.correoField;
            }
            set {
                if ((object.ReferenceEquals(this.correoField, value) != true)) {
                    this.correoField = value;
                    this.RaisePropertyChanged("correo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string icono {
            get {
                return this.iconoField;
            }
            set {
                if ((object.ReferenceEquals(this.iconoField, value) != true)) {
                    this.iconoField = value;
                    this.RaisePropertyChanged("icono");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nombre {
            get {
                return this.nombreField;
            }
            set {
                if ((object.ReferenceEquals(this.nombreField, value) != true)) {
                    this.nombreField = value;
                    this.RaisePropertyChanged("nombre");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nombreUsuario {
            get {
                return this.nombreUsuarioField;
            }
            set {
                if ((object.ReferenceEquals(this.nombreUsuarioField, value) != true)) {
                    this.nombreUsuarioField = value;
                    this.RaisePropertyChanged("nombreUsuario");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tipo {
            get {
                return this.tipoField;
            }
            set {
                if ((object.ReferenceEquals(this.tipoField, value) != true)) {
                    this.tipoField = value;
                    this.RaisePropertyChanged("tipo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ManejadorExcepciones", Namespace="http://schemas.datacontract.org/2004/07/BlbibliotecaClases")]
    [System.SerializableAttribute()]
    public partial class ManejadorExcepciones : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string mensajeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                if ((object.ReferenceEquals(this.mensajeField, value) != true)) {
                    this.mensajeField = value;
                    this.RaisePropertyChanged("mensaje");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioGloom.IServicioAdministrador", CallbackContract=typeof(ClienteGloomApp.ServicioGloom.IServicioAdministradorCallback))]
    public interface IServicioAdministrador {
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
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioGloom.IJugador")]
    public interface IJugador {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/AgregarJugador", ReplyAction="http://tempuri.org/IJugador/AgregarJugadorResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClienteGloomApp.ServicioGloom.ManejadorExcepciones), Action="http://tempuri.org/IJugador/AgregarJugadorManejadorExcepcionesFault", Name="ManejadorExcepciones", Namespace="http://schemas.datacontract.org/2004/07/BlbibliotecaClases")]
        int AgregarJugador(ClienteGloomApp.ServicioGloom.Jugador jugador);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/AgregarJugador", ReplyAction="http://tempuri.org/IJugador/AgregarJugadorResponse")]
        System.Threading.Tasks.Task<int> AgregarJugadorAsync(ClienteGloomApp.ServicioGloom.Jugador jugador);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/ActualizarJugador", ReplyAction="http://tempuri.org/IJugador/ActualizarJugadorResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClienteGloomApp.ServicioGloom.ManejadorExcepciones), Action="http://tempuri.org/IJugador/ActualizarJugadorManejadorExcepcionesFault", Name="ManejadorExcepciones", Namespace="http://schemas.datacontract.org/2004/07/BlbibliotecaClases")]
        int ActualizarJugador(ClienteGloomApp.ServicioGloom.Jugador jugador);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/ActualizarJugador", ReplyAction="http://tempuri.org/IJugador/ActualizarJugadorResponse")]
        System.Threading.Tasks.Task<int> ActualizarJugadorAsync(ClienteGloomApp.ServicioGloom.Jugador jugador);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/AutenticarJugador", ReplyAction="http://tempuri.org/IJugador/AutenticarJugadorResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClienteGloomApp.ServicioGloom.ManejadorExcepciones), Action="http://tempuri.org/IJugador/AutenticarJugadorManejadorExcepcionesFault", Name="ManejadorExcepciones", Namespace="http://schemas.datacontract.org/2004/07/BlbibliotecaClases")]
        int AutenticarJugador(ClienteGloomApp.ServicioGloom.Jugador jugador);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/AutenticarJugador", ReplyAction="http://tempuri.org/IJugador/AutenticarJugadorResponse")]
        System.Threading.Tasks.Task<int> AutenticarJugadorAsync(ClienteGloomApp.ServicioGloom.Jugador jugador);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/ObtenerJugador", ReplyAction="http://tempuri.org/IJugador/ObtenerJugadorResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(ClienteGloomApp.ServicioGloom.ManejadorExcepciones), Action="http://tempuri.org/IJugador/ObtenerJugadorManejadorExcepcionesFault", Name="ManejadorExcepciones", Namespace="http://schemas.datacontract.org/2004/07/BlbibliotecaClases")]
        ClienteGloomApp.ServicioGloom.Jugador ObtenerJugador(string nombreUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJugador/ObtenerJugador", ReplyAction="http://tempuri.org/IJugador/ObtenerJugadorResponse")]
        System.Threading.Tasks.Task<ClienteGloomApp.ServicioGloom.Jugador> ObtenerJugadorAsync(string nombreUsuario);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJugadorChannel : ClienteGloomApp.ServicioGloom.IJugador, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JugadorClient : System.ServiceModel.ClientBase<ClienteGloomApp.ServicioGloom.IJugador>, ClienteGloomApp.ServicioGloom.IJugador {
        
        public JugadorClient(System.ServiceModel.InstanceContext contextoJugador) {
        }
        
        public JugadorClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JugadorClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JugadorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JugadorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int AgregarJugador(ClienteGloomApp.ServicioGloom.Jugador jugador) {
            return base.Channel.AgregarJugador(jugador);
        }
        
        public System.Threading.Tasks.Task<int> AgregarJugadorAsync(ClienteGloomApp.ServicioGloom.Jugador jugador) {
            return base.Channel.AgregarJugadorAsync(jugador);
        }
        
        public int ActualizarJugador(ClienteGloomApp.ServicioGloom.Jugador jugador) {
            return base.Channel.ActualizarJugador(jugador);
        }
        
        public System.Threading.Tasks.Task<int> ActualizarJugadorAsync(ClienteGloomApp.ServicioGloom.Jugador jugador) {
            return base.Channel.ActualizarJugadorAsync(jugador);
        }
        
        public int AutenticarJugador(ClienteGloomApp.ServicioGloom.Jugador jugador) {
            return base.Channel.AutenticarJugador(jugador);
        }
        
        public System.Threading.Tasks.Task<int> AutenticarJugadorAsync(ClienteGloomApp.ServicioGloom.Jugador jugador) {
            return base.Channel.AutenticarJugadorAsync(jugador);
        }
        
        public ClienteGloomApp.ServicioGloom.Jugador ObtenerJugador(string nombreUsuario) {
            return base.Channel.ObtenerJugador(nombreUsuario);
        }
        
        public System.Threading.Tasks.Task<ClienteGloomApp.ServicioGloom.Jugador> ObtenerJugadorAsync(string nombreUsuario) {
            return base.Channel.ObtenerJugadorAsync(nombreUsuario);
        }
    }
}
