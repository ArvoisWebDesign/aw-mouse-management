$(document).ready(function () {
    let navLinks = $(".nav-link");
    for (let navLink of navLinks) {
        if ($(navLink).attr("href") == window.location.pathname)
            $(navLink).addClass("activeNavLink")
    }

    $("select").selectize({
        plugins: ["remove_button"]
    })
});

function mapCheckboxToInput(checkboxSelector, inputSelector) {
    $(checkboxSelector).change(function () {
        if ($(checkboxSelector).is(":checked"))
            $(inputSelector).val("true")
        else
            $(inputSelector).val("false")
    })
}

function mapFileInputToInput(fileInputSelector, inputSelector) {
    $(fileInputSelector).change(function () {
        let file = $(fileInputSelector).get(0).files[0]

        if (file != null) {
            let reader = new FileReader()
            reader.onloadend = function () {
                if (file.size <= 1048576) {
                    let photoAsString = reader.result.split(',')[1]
                    $(inputSelector).val(photoAsString)
                } else
                    $(fileInputSelector).val(null)
            }
            reader.readAsDataURL(file)
        }
    })
}

function addEventAutoPatternAntiSpecialCharacters(fieldSelector) {
    $(fieldSelector).on("dragover", false).on("keyup keydown drop", function (e) {
        // only letters, numbers, dot and hyphen
        let regex = /[^a-zA-Z0-9.-]+/g

        let oldValue = $(fieldSelector).val()
        let newValue = oldValue.replace(regex, "")

        $(fieldSelector).val(newValue)
    })
}