﻿; (function (VIS, $) {
    // GenralAttributeForm form declraion for constructor class
    function GenralAttributeForm(VAM_PFeature_SetInstance_ID, VADMS_AttributeSet_ID, windowNoP, searchP, canSaveRecord, fromDMS) {

        this.onClose = null;
        var $self = this;
        var $root = $("<div>");
        // var $busyDiv = $("<div class='vis-apanel-busy'>")
        this.log = VIS.Logging.VLogger.getVLogger("GenralAttributeForm");

        var windowNo = VIS.Env.getWindowNo();
        var MVAMPFeatureSetInstanceId = null;
        var MVAMPFeatureSetInstanceName = null;
        var cBPartnerId = null;
        var windowNoParent = windowNoP;
        var vadms_AttributeSet_ID = 0;
        var changed = false;
        var INSTANCE_VALUE_LENGTH = 40;
        var genAttSetInstance = null;
        var dms = fromDMS;
        var attributesList = {};
        var search = false;

        var Okbtn = null;
        var cancelbtn = null;
        var btnSelect = null;
        var controlList = null;

        this.log.config("VAB_GenFeatureSetInstance_ID=" + VAM_PFeature_SetInstance_ID);

        //constructor load
        MVAMPFeatureSetInstanceId = VAM_PFeature_SetInstance_ID;
        vadms_AttributeSet_ID = VADMS_AttributeSet_ID;
        dms = fromDMS;
        search = searchP;

        init();

        function init() {
            $.ajax({
                url: VIS.Application.contextUrl + "GenralAttribute/Load",
                dataType: "json",
                async: false,
                data: {
                    MVAMPFeatureSetInstanceId: MVAMPFeatureSetInstanceId,
                    vadms_AttributeSet_ID: vadms_AttributeSet_ID,
                    windowNo: windowNo,
                },
                success: function (data) {
                    returnValue = data.result;
                    VIS.Env.getCtx().setContext(windowNo, "VAB_GenFeatureSet_ID", vadms_AttributeSet_ID);
                    if (returnValue.Error != null) {
                        VIS.ADialog.error(returnValue.Error, null, null, null);
                        return;
                    }
                    //load div
                    $root.html(returnValue.tableStucture);
                    controlList = returnValue.ControlList.split(',');
                }
            });
        };

        //initialize control varibales after load from root
        //Find Fontrols
        Okbtn = $root.find("#btnOk_" + windowNo);
        cancelbtn = $root.find("#btnCancel_" + windowNo);

        if (canSaveRecord) {
            Okbtn.attr('disabled', false);
            Okbtn.css('opacity', '1');
        }
        else {
            Okbtn.attr('disabled', true);
            Okbtn.css('opacity', '0.6');
        }

        //check Arebic Calture
        if (VIS.Application.isRTL) {
            //Okbtn.css("margin-right", "-132px");
            cancelbtn.css("margin-right", "10px");
        }

        this.btnSelect = $root.find("#btnSelect_" + windowNo);
        this.txtDescription = $root.find("#txtDescription_" + windowNo);

        //Control that genrate run time get for first attribute

        function saveSelection() {
            //get all controls values into it
            var lst = [];
            for (var i = 0; i < controlList.length; i++) {
                var cntrl = $root.find("#" + controlList[i]);
                if (controlList[i].toString().contains("cmb")) {
                    lst.push({ 'Key': Number(cntrl.find('option:selected').val()), 'Name': cntrl.find('option:selected').text() });
                }
                else {
                    lst.push({ 'Key': 0, 'Name': VIS.Utility.decodeText(cntrl.val()) });
                }
            }
            $.ajax({
                url: VIS.Application.contextUrl + "GenralAttribute/Save",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
                data: JSON.stringify({
                    windowNoParent: windowNoParent,
                    MVAMPFeatureSetInstanceId: MVAMPFeatureSetInstanceId,
                    vadms_AttributeSet_ID: vadms_AttributeSet_ID,
                    windowNo: windowNo,
                    lst: lst
                }),
                success: function (data) {
                    returnValue = data.result;
                    if ($self.onClose) {
                        $self.onClose(returnValue.VAM_PFeature_SetInstance_ID, returnValue.VAM_PFeature_SetInstanceName);
                    }
                }
            });
        }

        function setSearchValues() {
            //get all controls values into it
            var lst = [];
            for (var i = 0; i < controlList.length; i++) {
                var cntrl = $root.find("#" + controlList[i]);
                if (controlList[i].toString().contains("cmb")) {
                    lst.push({ 'Key': Number(cntrl.find('option:selected').val()), 'Name': cntrl.find('option:selected').text() });
                }
                else {
                    lst.push({ 'Key': 0, 'Name': cntrl.val() });
                }
            }

            $.ajax({
                url: VIS.Application.contextUrl + "GenralAttribute/SearchValues",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
                data: JSON.stringify({
                    windowNoParent: Number(windowNoParent),
                    MVAMPFeatureSetInstanceId: MVAMPFeatureSetInstanceId,
                    vadms_AttributeSet_ID: vadms_AttributeSet_ID,
                    windowNo: windowNo,
                    lst: lst
                }),
                success: function (data) {
                    returnValue = data.result;
                    if ($self.onClose) {
                        if (returnValue.VAM_PFeature_SetInstanceName == null) {

                            returnValue.VAM_PFeature_SetInstanceName = returnValue.Description;
                        }
                        $self.onClose(returnValue.VAM_PFeature_SetInstance_ID, returnValue.VAM_PFeature_SetInstanceName, returnValue.GenSetInstance);
                    }
                }
            });
        }



        Okbtn.on("click", function () {
            if (search) {
                setSearchValues();
            }
            else {
                saveSelection();
            }

            $root.dialog('close');
        });

        cancelbtn.on("click", function () {
            $root.dialog('close');
        });


        this.showDialog = function () {
            $root.dialog({
                modal: true,
                title: VIS.Msg.translate(VIS.Env.getCtx(), "VAB_GenFeatureSetInstance_ID"),
                width: 400,
                close: function () {
                    $self.dispose();
                    $self = null;
                    $root.dialog("destroy");
                    $root = null;
                }
            });
        };

        this.disposeComponent = function () {
            if (Okbtn)
                Okbtn.off("click");
            if (cancelbtn)
                cancelbtn.off("click");
            VIS.Env.clearWinContext(VIS.Env.getCtx(), windowNo);
            VIS.Env.getCtx().setContext(VIS.Env.WINDOW_INFO, VIS.Env.TAB_INFO, "VAM_PFeature_SetInstance_ID", MVAMPFeatureSetInstanceId);

            Okbtn = null;
            cancelbtn = null;
            mLocatorId = null;
            MVAMPFeatureSetInstanceName = null;
            mProductId = null;
            cBPartnerId = null;
            adColumnId = null;
            windowNoParent = null;
            productWindow = null;
            /**	Change							*/
            changed = null;
            INSTANCE_VALUE_LENGTH = 0;
            attributesList = null;


            //attributesList = null;
            //chkNewEdit = null;
            //btnSelect = null;
            //txtLotString = null;
            //cmbLot = null;
            //txtSerNo = null;
            //dtpicGuaranteeDate = null;
            //txtDescription = null;


            $self = null;
            windowNo = null;
            MVAMPFeatureSetInstanceId = null;
            this.disposeComponent = null;
        };

    };

    //dispose call
    GenralAttributeForm.prototype.dispose = function () {
        this.disposeComponent();
    };

    VIS.GenralAttributeForm = GenralAttributeForm;
})(VIS, jQuery);