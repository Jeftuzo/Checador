window.lectorHuellas = {
    dotNetRef: null,

    iniciar: function (dotNetHelper) {
        this.dotNetRef = dotNetHelper;
        console.log("Iniciando modo compatible de DigitalPersona...");

        if (typeof Fingerprint !== 'undefined') {
            const api = new Fingerprint.WebApi();

            api.onSamplesAcquired = (event) => {
                this.dotNetRef.invokeMethodAsync('HuellaCapturada', "HUELLA_RECIBIDA");
                console.log("Huella detectada por evento de hardware.");
            };

            api.startAcquisition(Fingerprint.SampleFormat.PngImage)
                .then(() => console.log("Lector activo."))
                .catch(err => console.error("Error de adquisición:", err));
        } else {
            console.error("El SDK de Fingerprint no está cargado.");
        }
    }
};