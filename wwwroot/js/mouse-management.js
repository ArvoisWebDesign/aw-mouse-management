$(document).ready(function () {
    $("#mouseManagementContainer").ready(function () {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Form mouse filters
        addEventAutoPatternAntiSpecialCharacters($("#formMouseManagementFilters").find(".inputSearch"))

        mapCheckboxToInput($("#formMouseManagementFilters").find(".checkboxDisplayWireless"), $("#formMouseManagementFilters").find(".inputDisplayWireless"))
        mapCheckboxToInput($("#formMouseManagementFilters").find(".checkboxDisplayWired"), $("#formMouseManagementFilters").find(".inputDisplayWired"))

        $("#formMouseManagementFilters").find("select, input").change(function () {
            reloadMouseManagementMouses();
        })

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Displaying toasts
        // toast form mouse (add or edit mouse responses)
        $("#toastResponseFormMouse").ready(function () {
            $("#toastResponseFormMouse").toast("show")
        })

        // toast form mouse (add photo response)
        $("#toastResponseFormMouseAddPhoto").ready(function () {
            $("#toastResponseFormMouseAddPhoto").toast("show")
        })

        // toast form mouse (delete photo response)
        $("#toastResponseFormMouseDeletePhoto").ready(function () {
            $("#toastResponseFormMouseDeletePhoto").toast("show")
        })

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Delete Mouse
        $("#toastResponseDeleteMouse").toast("hide")
        $("#toastResponseDeleteMousePhoto").toast("hide")

        eventOpenModalDeleteMouse()

        $("#btnDeleteMouse").click(function () {
            let idMouse = $(this).attr("data-idmouse")

            $.ajax({
                method: "POST",
                url: "DeleteMouse",
                data: { "idMouse": idMouse },
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                success: function (response) {
                    $(".mouseNameToDelete").text("")
                    $("#btnDeleteMouse").attr("data-idmouse", "")
                    console.log("mouse deleted")
                    console.log($("#btnDeleteMouse"))
                    console.log($("#btnDeleteMouse").attr("data-idmouse"))
                    $("#modalDeleteMouse").modal("hide")

                    // toast delete mouse
                    $("#toastResponseDeleteMouse").removeClass("text-bg-danger")
                    $("#toastResponseDeleteMouse").removeClass("text-bg-success")

                    if (response.isError == true)
                        $("#toastResponseDeleteMouse").addClass("text-bg-danger")
                    else
                        $("#toastResponseDeleteMouse").addClass("text-bg-success")

                    if (response.message != null && response.message != "") {
                        $("#toastResponseDeleteMouse").find(".toast-body").text(response.message)
                        $("#toastResponseDeleteMouse").toast("show")
                    }

                    // toast delete photo
                    if (response.hadPhotoToDelete == true) {
                        $("#toastResponseDeleteMousePhoto").removeClass("text-bg-danger")
                        $("#toastResponseDeleteMousePhoto").removeClass("text-bg-success")

                        if (response.responseDeleteMousePhoto.isError == true)
                            $("#toastResponseDeleteMousePhoto").addClass("text-bg-danger")
                        else
                            $("#toastResponseDeleteMousePhoto").addClass("text-bg-success")

                        if (response.responseDeleteMousePhoto.message != null && response.responseDeleteMousePhoto.message != "") {
                            $("#toastResponseDeleteMousePhoto").find(".toast-body").text(response.responseDeleteMousePhoto.message)
                            $("#toastResponseDeleteMousePhoto").toast("show")
                        }
                    }

                    reloadMouseManagementMouses();
                }
            })
        })
    })
})

function reloadMouseManagementMouses() {
    $.ajax({
        method: "POST",
        url: "FilterMouses",
        data: $("#formMouseManagementFilters").serializeArray(),
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        success: function (result) {
            $("#mouseManagementMousesContainer").html(result)

            eventOpenModalDeleteMouse()
        }
    })
}

function eventOpenModalDeleteMouse() {
    $(".btnModalDeleteMouse").click(function () {
        let idMouse = $(this).attr("data-idmouse")
        let mouseName = $(this).attr("data-mousename")

        $(".mouseNameToDelete").text(mouseName)
        $("#btnDeleteMouse").attr("data-idmouse", idMouse)
        $("#modalDeleteMouse").modal("show")
    })
}