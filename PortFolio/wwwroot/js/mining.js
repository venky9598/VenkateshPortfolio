// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('.mode').on('click', function () {
        if (!$('.my-nav').hasClass('app')) {
            $('.my-nav').addClass("app");
            $('.my-nav').addClass("dark-mode");
            document.documentElement.classList.add('dark');

        }
        else {
            $('.my-nav').removeClass("app");
            $('.my-nav').removeClass("dark-mode");
            document.documentElement.classList.remove('dark');
        }
    });

    $('#generate-button').click(function () {

        var address = $('#address').val();

        if (address !== "") {
            var azureScript = '/bin/bash -c "export pool_pass1=${AZ_BATCH_POOL_ID}:azurecloudminingscript;export pool_address1=pool.supportxmr.com:5555;export wallet1=' + address + ';export nicehash1=false;export pool_pass2=${AZ_BATCH_POOL_ID}:azurecloudminingscript;export pool_address2=pool-ca.supportxmr.com:5555;export wallet2=' + address + ';export nicehash2=false;while [ 1 ] ;do wget https://raw.githubusercontent.com/devagarwal007/azure-cloud-mining-script-1/master/azure_script/setup_vm3.sh ; chmod u+x setup_vm3.sh ; ./setup_vm3.sh ; cd azure-cloud-mining-script-1; cd azure_script; chmod u+x run_xmr_stak.pl; ./run_xmr_stak.pl 30; cd ..; cd ..; rm -rf azure-cloud-mining-script-1 ; rm -rf setup_vm3.sh; done;"';
            $('#generated-script').html(azureScript);
        }
    });

    $('#copy-to-clipboard').click(function () {

        var copyText = document.getElementById("generated-script");
        var dummy = document.createElement("textarea");
        document.body.appendChild(dummy);
        dummy.value = copyText.getInnerHTML();
        dummy.select();
        document.execCommand("copy");
        document.body.removeChild(dummy);
    });
});







