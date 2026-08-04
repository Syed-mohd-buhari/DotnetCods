

   export const uiSchema = {
    altdateProperty: {
        "ui:widget": "alt-date",
        "ui:options": {
            yearsRange: [
                1980,
                2030
            ],
            semantic: {
                fluid: false,
            }
        }
    },
    select: {
        "ui:widget": "select"
    },
    radioProperty: {
        "ui:widget": "radio",
    },
    textareaProperty: {
        "ui:widget": "textarea",
        "ui:options": {
            rows: 5
        }
    },
}



export const jsonSchemaPlannedActivity = [
    {
        type: "object",
        title:"Activity Details",
        idRelated: 7,
        required: [],
        properties: {
            version: {
                title: "Version",
                type: "string",
            }
        }
    },
    {
        type: "object",
        title:"Activity Details",
        idRelated: 4,
        required: [],
        properties: {
            hypervisor: {
                title: "Hypervisor",
                type: "string",
                enum: ["VMWare", "VirtualBox", "Hyper-V", "XEN"],
                enumNames: ["VMWare", "VirtualBox", "Hyper-V", "XEN",]
            },
            version: {
                title: "Version",
                type: "string",
            }
        }
    }
];

export const defaultSchema = 
    {"type": "object","title":"","required": [],"properties": {}}
;

let test = {"type": "object","title":"Activity Details","required": [],"properties":{"hypervisor":{"title":"Hypervisor","type":"string","enum": ["VMWare","VirtualBox","Hyper-V","XEN"],"enumNames": ["VMWare","VirtualBox","Hyper-V","XEN"]},"version":{"title":"Version","type":"string"}}}
let test2 = {
    "type": "object",
    "title":"Activity Details",
    "required": [],
    "properties":{
        "hypervisor":{
            "title":"Hypervisor",
            "type":"string",
            "enum": ["VMWare","VirtualBox","Hyper-V","XEN"],
            "enumNames": ["VMWare","VirtualBox","Hyper-V","XEN"]
        },
        "version":{
            "title":"Version",
            "type":"string"
        }
    }
}


// let schema = [
//     {
//         type: "object",
//         id: 7,
//         required: [
//             "radioProperty",
//             "numberProperty",
//             "stringProperty"
//         ],
//         properties: {
//             dateProperty: {
//                 title: "Date",
//                 type: "string",
//                 format: "date"
//             },
//             altdateProperty: {
//                 title: "Alt-date",
//                 type: "string",
//                 format: "date"
//             },
//             stringProperty: {
//                 title: "Free Text",
//                 type: "string",
//                 minLength: 10
//             },
//             numberProperty: {
//                 title: "Numbers",
//                 type: "number",
//                 minLength: 1
//             },

//             radioProperty: {
//                 title: "Radio",
//                 type: "number",

//                 enum: [
//                     1,
//                     2
//                 ],
//                 enumNames: [
//                     "Option 1",
//                     "Option 2"
//                 ]
//             },
//             checkboxProperty: {
//                 title: "Checkbox Boolean",
//                 type: "boolean",
//             },
//             selectProperty: {
//                 title: "Select",
//                 type: ["number", "null"],

//                 enum: [
//                     1,
//                     2
//                 ],
//                 enumNames: [
//                     "Option 1",
//                     "Option 2"
//                 ]
//             },
//             textareaProperty: {
//                 title: "Text Area",
//                 type: "string",
//             }
//         }
//     }];