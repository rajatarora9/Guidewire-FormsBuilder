
(function ($) {

    $.fn.initilizePager = function (options) {
        var parentTable = $(this);

        var settings = $.extend({
            currentPage: 1,
            totalRecords: 0,
            pageSize: 3,
            gridContainerWidth:602,
            headerStyle: '',
            gridStyle: '',
            rowStyle: '',
            showEditIcon: false,
            hideFooter: false,
            colStyle: [],
            customwidth: [],
            colWidth: 110,
            addLinkText: 'Record',
            removeItemCallback: function () { },
            addItemCallback: function () { },
            editItemCallBack: function () { }
            }, options);


                var currentPage = 0;
                var totalRecords = 0;
                var pageSize = 0;
                var gridContainerWidth = settings.gridContainerWidth;
                var nextPageClass = ".moveNext";
                var previousPageClass = ".movePrevious";
                var firstPageClass = ".moveFirst";
                var lastPageClass = ".moveLast";
                var inputPageNumber = ".inputPageNumber";
                var totalPageNumberLabel = ".totalPageLabel";
                var hiddenClass = "hide";
                var deletedItemClass = "deleted";
                var removeIcon = ".removeItem";
        //TFS:290860 start- code added for sorting.
                var sortIcon = ".dynaGridFirstRow > .dynaGridHeaderText > .table-cell";
                var order = "A";
        //TFS:290860 end- code added for sorting.
                var addNewRecordsClass = ".addNewRecord";
                var editIcon = ".editItem";
                var headerStyle = "";
                var rowStyle = "";
                var colStyle = [];
                var colWidth = 120;
                var gridStyle = "";
                var addLinkText = "";             

                if (settings.hideFooter === true)
                    $(parentTable).find('.footerPanel').hide();

                if (settings.showEditIcon === true)
                    $(parentTable).find('.editItem').show();


                var getTotalRows = function () {
                    return $('div.dataRow', $(parentTable)).not('.' + deletedItemClass).length;
                }
                var getMaxPageNumber = function () {

                    return Math.ceil(getTotalRows() / pageSize);
                }
                var setInputPageNumber = function (pageNumber) {
                    $(inputPageNumber, $(parentTable)).val(pageNumber);

                }
                var renderGrid = function () {

                    var dataRows = $('div.dataRow', $(parentTable)).not('.' + deletedItemClass);
                    var endRecordsNum = currentPage * pageSize-1;
                    var startRecodNum = (endRecordsNum - pageSize) + 1;
                    if (currentPage === 1) {
                        startRecodNum = 0;
                    }
                    dataRows.each(function () {
                        if (!$(this).hasClass(hiddenClass)) {
                            $(this).addClass(hiddenClass);
                        }
                        $(this).css({ "padding-bottom": "0px" });
                    });

                    //if (dataRows.length === 0) {
                    // $('div.dynaGridHeaderContainer', $(parentTable)).css({ "padding-bottom": "20px" });
                    //}
                    //else {
                    $('div.dynaGridHeaderContainer', $(parentTable)).css({ "padding-bottom": "5px" });
                    //}
                    var lastRow = dataRows.last();
                    lastRow.css({ "padding-bottom": "20px" });
                    dataRows.slice(startRecodNum, endRecordsNum + 1).removeClass(hiddenClass);

                    setInputPageNumber(currentPage);

                }
                var moveToNextPage = function () {
                    if (currentPage < getMaxPageNumber()) {
                        currentPage++;
                        renderGrid();
                    }


                }
                var moveToPreviousPage = function () {
                    if (currentPage > 1 && getMaxPageNumber() > 1) {
                        currentPage--;
                        renderGrid();
                    }


                }

                var moveToFirstPage = function () {
                    currentPage = 1;
                    renderGrid();
                }
                var moveToLastPage = function () {
                    if (getMaxPageNumber() <= 1) {
                        currentPage = 1;
                    } else {
                        currentPage = getMaxPageNumber();

                    }
                    renderGrid();
                }
                var initilizePager = function () {

                    currentPage = settings.currentPage;
                    pageSize = settings.pageSize;
                    totalRecords = getTotalRows();
                    headerStyle = settings.headerStyle;
                    rowStyle = settings.rowStyle;
                    colStyle = settings.colStyle;
                    gridStyle = settings.gridStyle;
                    addLinkText = settings.addLinkText;
                    colWidth = settings.colWidth;
                    customwidth = settings.customwidth;
                    customwidth = settings.customwidth;
                  
                }

                var setTotalCountLabelContent = function () {
                    var maxPageNumber = getMaxPageNumber();
                    $(totalPageNumberLabel, $(parentTable)).html('of ' + (maxPageNumber > 0 ? maxPageNumber : 1));
                }

                var removeRow = function () {

                    //this has been added to for parties summary screen where some records in dyna grid prevents user 
                    //from deleting some record- rajdeep sharma
                    if ($(this).hasClass('isDisabled')) {
                        return false;
                    }

                    $(this).closest('div.dataRow').addClass(deletedItemClass).addClass(hiddenClass);
                    $('input[name*="IsFormDirty"]').val(true);
                    if ($('div.dataRow:visible', $(parentTable)).length < 1) {
                        if (currentPage > 1 && getMaxPageNumber() > 1) {
                            currentPage--;
                        }
                        if (currentPage > 1 && getMaxPageNumber() === 1) {
                            currentPage = 1;
                        }
                    }
                    if ($('div.dataRow:visible', $(parentTable)).length === pageSize || $('div.dataRow:visible', $(parentTable)).length < pageSize) {
                        currentPage = 1;
                    }
                    renderGrid();
                    setTotalCountLabelContent();
                    if (settings.removeItemCallback) {
                        settings.removeItemCallback($(this));
                    }
                }

        //TFS:290860 start- code added for sorting.
                var sortColumn = function () {

                    if (!($(this).find('.isSortingRequired').length == 1)) //($(this).hasClass("isSortingRequired")()) // $( "#mydiv" ).hasClass( "foo" )
                    {
                        //If 'isSortingRequired' Class is not present, Don't perform sorting and exit the function.
                       return false;
                    }

                    var dynagridcolumns = $(this).parent().parent().find(".dynaGridHeaderText > .table-cell");
                    var dynagridrows = $(this).parent().parent().find(".dynaGridCellText");
                    //var order = "A";                    
                    var columnclicked = "";
                    // handle click and add class
                    //dynagridcolumns.on("click", function () {
                    var columnClass = ".Column" + (dynagridcolumns.index(this));
                    columnclicked = dynagridcolumns.index(this);

                    if (order == "A") {
                        var cells = $(dynagridrows[0]).find(".gridDataRow > .table-cell");
                        if ($(cells[columnclicked]).find(".comboBoxBaseStyle").length > 0) {
                            dynagridrows = dynagridrows.sort(function (a, b) {
                                var aKBPS = $(columnClass, a).find(".comboBoxBaseStyle option:selected").text(),
                                    bKBPS = $(columnClass, b).find(".comboBoxBaseStyle option:selected").text()
                                return bKBPS > aKBPS ? -1 : aKBPS > bKBPS ? 1 : 0
                            });
                        }
                        else if ($(cells[columnclicked]).find(".datebox").length > 0) {
                            dynagridrows = dynagridrows.sort(function (a, b) {
                                var aKBPS = Date.parse($(columnClass, a).find(".datebox").val()),
                                    bKBPS = Date.parse($(columnClass, b).find(".datebox").val())
                                return bKBPS > aKBPS ? -1 : aKBPS > bKBPS ? 1 : 0
                            });
                        }
                        else if ($(cells[columnclicked]).find(".vlabel").length > 0) {
                            dynagridrows = dynagridrows.sort(function (a, b) {
                                var aKBPS = $(columnClass, a).find(".vlabel").text(),
                                    bKBPS = $(columnClass, b).find(".vlabel").text()
                                return bKBPS > aKBPS ? -1 : aKBPS > bKBPS ? 1 : 0
                            });
                        }
                        $(this).parent().parent().find(".dynaGridCellText").detach();
                        $(this).parent().parent().parent().find(".dynaGridFirstRow").append($(dynagridrows));
                        order = "D";
                    }
                    else {
                        var cells = $(dynagridrows[0]).find(".gridDataRow > .table-cell");
                        if ($(cells[columnclicked]).find(".comboBoxBaseStyle").length > 0) {
                            dynagridrows = dynagridrows.sort(function (a, b) {
                                var aKBPS = $(columnClass, a).find(".comboBoxBaseStyle option:selected").text(),
                                    bKBPS = $(columnClass, b).find(".comboBoxBaseStyle option:selected").text()
                                return bKBPS < aKBPS ? -1 : aKBPS > bKBPS ? 1 : 0
                            });
                        }
                        else if ($(cells[columnclicked]).find(".datebox").length > 0) {
                            dynagridrows = dynagridrows.sort(function (a, b) {
                                var aKBPS = Date.parse($(columnClass, a).find(".datebox").val()),
                                    bKBPS = Date.parse($(columnClass, b).find(".datebox").val())
                                return bKBPS < aKBPS ? -1 : aKBPS > bKBPS ? 1 : 0
                            });
                        }
                        else if ($(cells[columnclicked]).find(".vlabel").length > 0) {
                            dynagridrows = dynagridrows.sort(function (a, b) {
                                var aKBPS = $(columnClass, a).find(".vlabel").text(),
                                    bKBPS = $(columnClass, b).find(".vlabel").text()
                                return bKBPS < aKBPS ? -1 : aKBPS > bKBPS ? 1 : 0
                            });
                        }
                        $(this).parent().parent().find(".dynaGridCellText").detach();
                        $(this).parent().parent().parent().find(".dynaGridFirstRow").append($(dynagridrows));
                        order = "A";
                    }
                    renderGrid();
                    applyStyles(settings.customwidth);
                    attacheValidation();
                    //}
                    //);
                }
        //TFS:290860 End- code added for sorting.

                var editItem = function () {
                    var row = $(this).closest('div.dataRow');
                    if (settings.editItemCallBack) {
                        settings.editItemCallBack($(row));
                    }
                }
                var addNewRowToGrid = function (e) {
           
                    var firstRow = $('div.dataRow', $(parentTable)).first();

                    var dataRow = $(firstRow).clone(true, true);
                    dataRow.removeClass(deletedItemClass);
           

                    //Moved the code from here to end of this function , If in case any issues are there eill move the code back here

                    //Moved this code from top to here in order to change f=dropdown default index to -1 , if in case any issues are there , move the code back
                    $(dataRow.find('input,select,textarea').not("[name$='Index'] ,[id$='minDate'] ,[id$='maxDate'] ,[id$='yearRange'] ,[id$='dateat'],[id$='Lookup'],[class$='ddlignore']")).each(function () {
                        var columnVal = "";
                        if ($(this).attr("type") === "hidden" && $(this).attr("data-defaultcolumnvalue")) {
                            columnVal = $(this).attr("data-defaultcolumnvalue");
                        }
                        if ($(this).attr('id') != undefined) {
                            if ($(this).attr('id').indexOf('IndividualIdsAsString') != -1) {
                                columnVal = $(this).attr("data-initialvalue");
                            }
                            if ($(this).attr('id').indexOf('DaysOfWeekScheduled') != -1) {
                                columnVal = $(this).attr("data-initialvalue");
                            }
                        }
                        $(this).val(columnVal);
                        
                    });


            $(dataRow.find('label,span').not("[class='disbaledToolTip'], [name$='Index'], [id$='unitID'],[id$='minDate'] ,[id$='maxDate'] ,[id$='yearRange'] ,[id$='dateFormat'],[id$='lblCheckBox']")).text("");
            dataRow.find(':checkbox').removeAttr('checked');

                    var indexArray = [];
                    $("[name$='Index']", $(parentTable)).each(function () {
                        indexArray.push(parseInt($(this).val()));
                    });
                    var nextIndex = Math.max.apply(Math, indexArray) + 1;
                    $($("[name$='Index']", dataRow)).val(nextIndex);

                    $('img.ui-datepicker-trigger', dataRow).remove();
                    $(firstRow).before(dataRow);

                    dataRow.find('input,select,label,span').each(function () {
                        var hasangularbracket = false;
                        if ($(this).attr('id') != undefined) {
                            var replacedValue = $(this).attr('id').replace(/\[(\d+)\](?!.*(\[(\d+)\]))/, function (str, p1) {
                                hasangularbracket = true;
                                return '[' + nextIndex + ']';
                            });
                            if ($(this).attr('id').indexOf('IndividualIdsAsString') != -1) {
                                replacedValue = $(this).attr('id').replace(/(\[(\d+)\])/, function (str, p1) {
                                    hasangularbracket = true;
                                    return '[' + nextIndex + ']';
                                });
                            }
                            if ($(this).attr('id').indexOf('DaysOfWeekScheduled') != -1) {
                                replacedValue = $(this).attr('id').replace(/(\[(\d+)\])/, function (str, p1) {
                                    hasangularbracket = true;
                                    return '[' + nextIndex + ']';
                                });
                            }

                            if (!hasangularbracket && !$(this).hasClass('datepicker')) {
                                var id = $(this).attr('id');
                                var lastUnderscoreIndex = id.lastIndexOf('_') ;
                                var subStringExcludingLastUnderscore = id.substring(0, lastUnderscoreIndex);
                                var splitIndex = subStringExcludingLastUnderscore.lastIndexOf('_');
                                var firstString = id.substring(0, splitIndex);
                                var secondString = id.substring(splitIndex, id.length);
                                secondString = secondString.replace(/\_(\d+)\_(?!.*(\_(\d+)\_))/, function (str, p1) { return '_' + nextIndex + '_'; });
                                replacedValue = firstString + secondString;
                            }
                            if ($(this).hasClass('datepicker')) {
                                var counter = nextIndex;
                                var positionOfLastUnderscore = replacedValue.lastIndexOf('_');
                                replacedValue = replacedValue.substring(0, positionOfLastUnderscore+1);
                                replacedValue = replacedValue + "" + counter;
                            }
                            $(this).attr('id', replacedValue);
                            if ($(this).attr('name') != undefined) {
                                $(this).attr('name', $(this).attr('name').replace(/\[(\d+)\](?!.*(\[(\d+)\]))/, function (str, p1) { return '[' + nextIndex + ']'; }));
                            }

                            if ($(this).hasClass('hasDatepicker')) {
                                $(this).removeClass('hasDatepicker').removeData('datepicker').unbind();
                                $(this).not('.datepickerMonthOnly').datepicker({
                                    showOn: "button",
                                    buttonImage: window.datepickerimgagesource,
                                    defaultDate: window.ApplicationDate,
                                    buttonImageOnly: true,
                                    changeMonth: true,
                                    changeYear: true,
                                    minDate: '-125Y',
                                    maxDate: '+125Y',
                                    yearRange: "-125:+125",
                                    onSelect: function (a, b, c) {
                                        //Added by Suresh Paldia
                                        //Bug Fix - #19286, #13526 - On Select of a date in datepicker for first time, required validation gets fired by default
                                        $(b.input).focus();
                                        $(b.input).blur();
                                    }
                                });
                                if ($(this).hasClass('datepickerMonthOnly'))
                                {
                                    $(this).attr("dateData", window.ApplicationDate);
                                    $(this).datepicker({
                                        showOn: "button",
                                        buttonImage: window.datepickerimgagesource,
                                        buttonImageOnly: true,
                                        changeMonth: true,
                                        changeYear: true,
                                        showButtonPanel: true,
                                        dateFormat: 'M yy',
                                        minDate: '-125Y',
                                        maxDate: '+125Y',
                                        yearRange: "-125:+125",
                                        beforeShow: function (dateText) {
                                            $(this).datepicker("option", "defaultDate", new Date($(this).attr("dateData")));
                                        },
                                        onClose: function (dateText, inst) {
                                            if (this.name.indexOf("IncreaseStartDate") != -1) {
                                                $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                                                $(this).attr("dateData", new Date(inst.selectedYear, inst.selectedMonth, 1));
                                            }
                                            if (this.name.indexOf("IncreaseEndDate") != -1) {
                                                $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth + 1, 0));
                                                $(this).attr("dateData", new Date(inst.selectedYear, inst.selectedMonth + 1, 0));
                                            }
                                        }
                                    });
                                }                        
                                $(this).addClass("datepicker hasDatepicker");
                                $(this).on("blur", function (event) { $(this).AutoFillDate(this); })
                                $(this).on("mouseenter", (function (event) { $(this).ShowCusotmToolTip(event); }))
                                $(this).on("mouseleave", (function (event) { $(this).HideCusotmToolTip(event); }))
                            }

                            if ($(this).hasClass("currencyText")) {
                                $(this).next( ".unit").text("$");
                            }
                            $(this).removeClass('input-validation-error custom-validation-error');
                        }
                    }
                    );

                    dataRow.find('select').each(function () {

                        $(this).prop('selectedIndex', -1);

                    });

                    if (settings.addItemCallback) {
                        settings.addItemCallback(dataRow);
                    }              
                    renderGrid();
                    setTotalCountLabelContent();
                    e.preventDefault();
                    $("form").removeData("validator");
                    $("form").removeData("unobtrusiveValidation");
                    //$.validator.unobtrusive.parse("form");

                }

                var setPageNumber = function (e) {

                    var value = $(this).val();
                    if (!(value < 1 || value > getMaxPageNumber())) {
                        currentPage = value;
                        renderGrid();

                    }

                }

                var restrictNumber = function (e) {

                    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||

                            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||

                            (e.keyCode >= 35 && e.keyCode <= 40)) {

                        return;
                    }
                    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                        e.preventDefault();
                    }
                }
                var applyMandatorySymbol = function ($e) {
                    var labelElement;

                    $($e).each(function () {

                        if ($(this).hasClass('datepicker')) {

                            labelElement = $("label[for*='" + $(this).attr('name') + "']", $(parentTable));

                        }
                        else {
                            labelElement = $("label[for$='" + $($(this).attr('id').split('.')).get(-1) + "']", $(parentTable));

                        }
                        if (labelElement.length > 0) {
                            return false;
                        }

                    })

                    var mandatoryElementString = "<span class=\"mandatory-sign red\" aria-required=\"true\">*</span>";
                    if (labelElement!= undefined && labelElement.length > 0) {
                        $(labelElement).after(mandatoryElementString);
                    }
                }
                var attacheValidation = function () {

                    if ($(parentTable).closest('.divParentGrid').attr('data-validationList') != undefined) {
                        var data = $.parseJSON($(parentTable).closest('.divParentGrid').attr('data-validationList'));
                        var visibleDatRows = $('.gridDataRow', $(parentTable).closest('.divParentGrid'));

                        for (var key in data) {
                            var mandatorySymbolApplied = false;

                            $(visibleDatRows).each(function () {
                                var element = $(this).find('[name*="' + key + '"]');
                                var keyElement = {};
                                if (element.length > 1) {
                                    $(element).each(function () {
                                        var name = $($(this).attr('name').split('.')).get(-1);
                                        if (name === key) {
                                            keyElement = $(this);
                                            return false;
                                        }
                                    });
                                } else {
                                    keyElement = element;
                                }
                                // if (keyElement.is(":visible")) {

                                for (var validation in data[key]) {

                                    if (validation === "apply-required-symbol" && !mandatorySymbolApplied && data[key]["apply-required-symbol"]) {
                                        //applyMandatorySymbol(keyElement);
                                        mandatorySymbolApplied = true;
                                    }

                                    $(keyElement).attr(validation, data[key][validation]);
                                }
                                //  }

                            });

                            $(parentTable).closest('.divParentGrid').removeAttr('data-validationList');

                        }
                        $("form").removeData("validator");
                        $("form").removeData("unobtrusiveValidation");
                        //$.validator.unobtrusive.parse("form");
                    }
                }

                var applyStyles = function (style) {
                    var data=style;
                    var containerWidth =$('.mainCenterPanel').width();
                    var width = "602";
                    var parentRow = $($(parentTable).closest('.table-row').prev('.table-row'));
                    var gridscrollWidth = gridContainerWidth + 20


                    var visibleColCount = $('div.table-cell', $('.table-header-Row', $(parentTable))).length;
                    containerWidth = width.toString() + 'px'

                    var fixedColumnWidth = colWidth.toString() + 'px';
                    var newwidth = 0;

                    for (var i = 0; i < data.length; i++) {
                        newwidth = newwidth + data[i];
                    }


                    // set container width
                    $(parentTable).css("width", gridContainerWidth + 'px');
                    $('.firstRow', $(parentTable)).css("width", newwidth + 70 + 'px');

                    $('#scrollDiv', $(parentTable)).css("width", gridscrollWidth + 'px');

                    $(('div:first'), $(parentTable)).addClass(gridStyle);

                    var gridHeader = $('.table-header-Row', $(parentTable));
                    $(gridHeader).css("width", newwidth + 70 + 'px');

                    var gridRow = $('.dataRow', $(parentTable));
                    var gridDataRow = $('.gridDataRow', $(parentTable));

                    if (newwidth > gridContainerWidth)
                    {
                        $('#scrollDiv' ,$(parentTable)).css("overflow-x", 'auto')
                        $('#scrollDiv', $(parentTable)).css("overflow-y", 'hidden')
                    }
                    $('.footerPanel', $(parentTable)).css("width", gridscrollWidth + 13 + 'px')

                    $('.hrRow', $(parentTable)).width(newwidth + 50 + 'px');

                    $(('a.addNewRecord'), $(parentTable)).html("+" + addLinkText);
                    for (var i = 0; i < visibleColCount; i++) {
                        $("div.table-cell:eq(" + i + ")", $(gridHeader)).css("width", data[i] + 'px');// fixedColumnWidth);
                    }

                    var gridHeaderLabels = $('.dynaGridHeaderContainer', $(parentTable)).find("label");

                    gridHeaderLabels.each(function () {
                        $(this).html($(this).html().replace("&lt;newlineplaceholder&gt;", "<br/>"));
                    })

                    $(".divParentGrid").parent().css("padding-left", "0px");

                    $(gridDataRow).each(function () {
                        var counter = 0;
                        for (var i = 0; i < visibleColCount ; i++) {
                            $("div.table-cell:eq(" + i + ")", $(this)).css("width", data[i] + 'px');// fixedColumnWidth);
                        }

                        $($(this).find('input').not("[name$='Index'] ,[id$='minDate'] ,[id$='maxDate'] ,[id$='yearRange'] ,[id$='dateat'],[id$='Lookup']")).css({ "margin-top": "0px" });
                    });
                }
                return this.each(function () {

                    initilizePager();
                    setInputPageNumber(currentPage);
                    setTotalCountLabelContent();
                    $(nextPageClass, $(parentTable)).on('click', moveToNextPage);
                    $(previousPageClass, $(parentTable)).on('click', moveToPreviousPage);
                    $(lastPageClass, $(parentTable)).on('click', moveToLastPage);
                    $(firstPageClass, $(parentTable)).on('click', moveToFirstPage);
                    $(removeIcon, $(parentTable)).on('click', removeRow);
                    $(inputPageNumber, $(parentTable)).on('focusout', setPageNumber);
                    $(inputPageNumber, $(parentTable)).on('keydown', restrictNumber);
                    $(addNewRecordsClass, $(parentTable)).on('click', addNewRowToGrid);
                    $(editIcon, $(parentTable)).on('click', editItem);
                    $(sortIcon, $(parentTable)).on('click', sortColumn); //TFS:290860 - code added for sorting.
                    renderGrid();
                    applyStyles(settings.customwidth);
                    attacheValidation();
                });

            };

}(jQuery));
