    /* Constants */
    /* Block Credit Limits */
    var const_BlockCreditMin = 12;
    var const_BlockCreditMax = 18;

    /* In State Per Hour Tuition Rate */
    var const_InStateFreshman = 482;
    var const_InStateSophmore = 494;
    var const_InStateJunior = 555;
    var const_InStateSenior = 555;

    var const_InStateMasters = 785.75;
    var const_InStateDoctoral = 785.75;

    var const_InStateEnglishLanguageCenter = 482;
    var const_InStateTeacherCertification = 670.50;
    var const_InStateLifeLongEducation = 785.75;
    var const_InStateEducationGraduate = 818.75;
    var const_InStateEngineeringGraduate = 875.75;
    var const_InStateAccountingGraduate = 989.75;

    var const_InStateBusinessFreshman = 482;
    var const_InStateBusinessSophmore = 494;
    var const_InStateBusinessJunior = 573;
    var const_InStateBusinessSenior = 573;

    var const_InStateEngineeringFreshman = 482;
    var const_InStateEngineeringSophmore = 494;
    var const_InStateEngineeringJunior = 573;
    var const_InStateEngineeringSenior = 573;

    var const_InStateProfessionalFreshman = 482;
    var const_InStateProfessionalSophmore = 494;
    var const_InStateProfessionalJunior = 555;
    var const_InStateProfessionalSenior = 555;

    var const_InStateHumanMedicine = 15656;
    var const_InStateOsteopathicMedicine = 15656;
    var const_InStateVeternaryMedicineClinical = 13984;
    var const_InStateVeternaryMedicineNonClinical = 15814;


    /* Out State Per Hour Tuition Rate */
    var const_OutStateFreshman = 1325.50;
    var const_OutStateSophmore = 1325.50;
    var const_OutStateJunior = 1366.75;
    var const_OutStateSenior = 1366.75;

    var const_OutStateMasters = 1544;
    var const_OutStateDoctoral = 1544;

    var const_OutStateEnglishLanguageCenter = 1325.50;
    var const_OutStateTeacherCertification = 1455.50;
    var const_OutStateLifeLongEducation = 1007;
    var const_OutStateEducationGraduate = 1577;
    var const_OutStateEngineeringGraduate = 1641;
    var const_OutStateAccountingGraduate = 1788;

    var const_OutStateBusinessFreshman = 1325.50;
    var const_OutStateBusinessSophmore = 1325.50;
    var const_OutStateBusinessJunior = 1385.75;
    var const_OutStateBusinessSenior = 1385.75;

    var const_OutStateEngineeringFreshman = 1325.50;
    var const_OutStateEngineeringSophmore = 1325.50;
    var const_OutStateEngineeringJunior = 1385.75;
    var const_OutStateEngineeringSenior = 1385.75;

    var const_OutStateProfessionalFreshman = 1325.50;
    var const_OutStateProfessionalSophmore = 1325.50;
    var const_OutStateProfessionalJunior = 1366.75;
    var const_OutStateProfessionalSenior = 1366.75;

    var const_OutStateHumanMedicine = 29033;
    var const_OutStateOsteopathicMedicine = 29033;
    var const_OutStateVeternaryMedicineClinical = 25138;
    var const_OutStateVeternaryMedicineNonClinical = 28235;

    /* Perform Calculations */
    function Calculate() {
        var costTuition = 0;
        var costInternationalStudentFee = 0;
        var costCollegeFee = 0;
        var costMajorFee = 0;
        var costGradTax = 0;
        var costAsmsuTax = 0;
        var costRadioTax = 0;
        var costStateNewsTax = 0;
        var costCollegeTax = 0;
        var costHousing = 0;
        var costHousingTax = 0;
        var costTotal = 0;

        var residency = $('#residency').val();
        var level = $('#level').val();
        var college = $('#college').val();
        var major = $('#major').val();
        var credits = $('#credits').val();
        var housing = $('#housing').val();
        var subHousing = $('#subHousing').val();
        var isAgTech = $('#isAgTech').is(':checked');
        var isEngAccepted = $('#isEngAccepted').is(':checked');
        var isVetClinicalYear = $('#isVetClinicalYear').is(':checked');

        /* Insert Validation Processing Here */
        if (isNaN(credits)) {
            credits = 0;
        }

        costTuition = GetTuition(residency, level, college, credits, isAgTech, isVetClinicalYear);
        costInternationalStudentFee = GetInternationalFee(residency, level, credits);
        costCollegeFee = GetCollegeFee(level, college, isEngAccepted, credits);
        costMajorFee = GetMajorFee(level, college, major, credits);
        costGradTax = GetGradTax(level, college);
        costAsmsuTax = GetASMSUTax(level, isAgTech, credits)
        costRadioTax = GetRadioTax(level, credits);
        costStateNewsTax = GetStateNewsTax(level, credits);
        costCollegeTax = GetCollegeTax(college);
        costHousing = GetHousing(housing, subHousing);
        costHousingTax = GetHousingTax(housing, subHousing);
        costMedicalTax = GetMedicalTax(level)


        costTotal = costTuition + costInternationalStudentFee + costCollegeFee + costMajorFee + costGradTax + costAsmsuTax + costRadioTax + costStateNewsTax + costCollegeTax + costHousing + costHousingTax + costMedicalTax ;

        return costTotal;
    }

    /* 
    *  Tuition is essentially (credits * rate); however, the number of credits may be adjusted
    *  because of block tuition. This function first figures the adjusted number of credits and
    *  then determines the per credit rate and multiplies the two to get the tuition total.
    */
    function GetTuition(residency, level, college, credits, isAgTech, isVetClinicalYear) {

        var creditCount = GetCreditCount(credits, level, isAgTech);
        var hourlyRate = GetPerHourRate(residency, level, college, isVetClinicalYear);
        var blockAdjustment = GetBlockAdjustment(residency, level, credits);

        return (creditCount * hourlyRate) + blockAdjustment;
    }

    function GetInternationalFee(residency, level, credits) {

        //early exit conditions
        if (residency !== 'International') {
            return 0;
        }

        switch (level) {
            case 'Freshman':
            case 'Sophmore':
            case 'Junior':
            case 'Senior':
                switch (credits) {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                        return 375.00;
                    default:
                        return 750.00;
                }
            case 'Teacher Certification Intern':
            case 'Masters':
            case 'Doctoral':
            case 'Medical Professinal':
                switch (credits) {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                        return 37.50;
                    default:
                        return 75.00;
                }
            case 'English Language Center':
            case 'Lifelong Student':
            default:
                return 0;
        }
    }

    function GetCollegeFee(level, college, isEngAccepted, credits) {

        var collegeType = GetCollegeType(college);

        if (collegeType === 'business' && (level === 'Junior' || level === 'Senior')) {
            switch (credits) {
                case '1':
                case '2':
                case '3':
                case '4':
                    return 113;
                default:
                    return 226;
            }
        } else if (collegeType === 'engineering' && (level === 'Junior' || level === 'Senior' || isEngAccepted === true)) {
            switch (credits) {
                case '1':
                case '2':
                case '3':
                case '4':
                    return 402;
                default:
                    return 670;
            }
        } else {
            return 0;
        }
    }

    function GetMajorFee(level, college, major, credits) {

        if (level === 'Junior' || level === 'Senior') {
            var majorType = GetMajorType(college, major);

            switch (majorType) {
                case 'science':
                    switch (credits) {
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                            return 50;
                        default:
                            return 100;
                    }
                case 'health':
                    switch (credits) {
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                            return 50;
                        default:
                            return 100;
                    }
                default:
                    return 0;
            }
        }

        return 0;
    }

    function GetGradTax(level, college) {
        if (level === 'Masters' || level === 'Doctoral' || level === 'Medical Professional') {
            return 11;
        } else {
            return 0;
        }
    }

    function GetASMSUTax(level, isAgTech, credits) {

        if (credits > 0 && (level === "Freshman" || level === "Sophmore" || level === "Junior" || level === "Senior" || isAgTech === true || level === 'English Language Center')) {
            return 21;
        } else {
            return 0;
        }
    }

    function GetRadioTax(level, credits) {
        if (credits === 0) {
            return 0;
        }
        if (level === "Freshman" ||
            level === "Sophmore" ||
            level === "Junior" ||
            level === "Senior" ||
            level === "Masters" ||
            level === "Doctoral" ||
            level === "English Language Center" ||
            level === "Medical Professional"
        ) {
            return 3;
        }

        return 0;
    }

    function GetStateNewsTax(level, credits) {

        if (level === "Freshman" ||
            level === "Sophmore" ||
            level === "Junior" ||
            level === "Senior" ||
            level === "Masters" ||
            level === "Doctoral" ||
            level === "English Language Center" ||
            level === "Medical Professional") {
            switch (credits) {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                    return 0;
                default:
                    return 7.5;
            }
        } else {
            return 0;
        }
    }

    function GetCollegeTax(college) {
        if (college === "James Madison College") {
            return 2;
        } else {
            return 0;
        }
    }
    function GetMedicalTax(level) {
        if (level === "Medical Professional") {
            return 1.5;
        }
        else {
            return 0;
        }

    }

    function GetHousing(housing, subHousing) {

        switch (housing) {
            case 'No Plan Chosen':
                return 0;
            case 'Single Room With Platinum':
                return 6630;
            case 'Single Room With Gold':
                return 6430;
            case 'Single Room With Silver':
                return 6030;
            case 'Double Room With Platinum':
                return 5536;
            case 'Double Room With Gold':
                return 5386;
            case 'Double Room With Silver':
                return 5236;
            case 'Akers & West Circle, Room Only, Double':
                return 2669;
            case 'Owen Graduate Center, Permanent Single, Room Only':
                return 3136;
            case 'Owen Graduate Center, Designated Single, Room Only':
                return 3656;
            case 'Owen Graduate Center, Permanent Single, Meal Plan':
                return 5912;
            case 'Owen Graduate Center, Designated Single, Meal Plan':
                return 6432;
            case 'Van Hoosen Apartments, Room Only, Single':
                return 6312;
            case 'Van Hoosen Apartments, Room Only, Double':
                return 3156;
            case 'Van Hoosen Apartments, Room Only, Triple':
                return 2104;
            case 'Van Hoosen Apartments, Room Only, Quad':
                return 1578;
            case "William's Hall, Single, Room Only":
                return 3750;
            case "William's Hall, Double, Room Only":
                return 2414;
            case "William's Hall, Single, Meal Plan":
                return 6526;
            case "William's Hall, Double, Meal Plan":
                return 5190;
            case 'University Village Apartments':
                return 3700;
            case '1855 Place':
                switch (subHousing) {
                    case 'Studio':
                        return 4975;
                    case 'Two Bedroom':
                        return 4520;
                    case 'Two Bedroom Efficiency':
                        return 4420;
                    case 'Four Bedroom Flat':
                        return 4090;
                    case 'Four Bedroom Efficiency':
                        return 3965;
                    case 'Four Bedroom Townhouse':
                        return 4295;
                    case 'Family Apartments, One Bedroom':
                        return 4040;
                    case 'Family Apartments, Two Bedroom':
                        return 4670;
                    default:
                        return 0;
                };
            default:
                return 0;
        }
    }


    function GetHousingTax(housing, subHousing) {
        switch (housing) {
            case 'No Plan Chosen':
                return 0;
            case 'Single Room With Platinum':
                return 25;
            case 'Single Room With Gold':
                return 25;
            case 'Single Room With Silver':
                return 25;
            case 'Double Room With Platinum':
                return 25;
            case 'Double Room With Gold':
                return 25;
            case 'Double Room With Silver':
                return 25;
            case 'Akers & West Circle, Room Only, Double':
                return 25;
            case 'Owen Graduate Center, Permanent Single, Room Only':
                return 8;
            case 'Owen Graduate Center, Designated Single, Room Only':
                return 8;
            case 'Owen Graduate Center, Permanent Single, Meal Plan':
                return 8;
            case 'Owen Graduate Center, Designated Single, Meal Plan':
                return 8;
            case 'Van Hoosen Apartments, Room Only, Single':
                return 25;
            case 'Van Hoosen Apartments, Room Only, Double':
                return 25;
            case 'Van Hoosen Apartments, Room Only, Triple':
                return 25;
            case 'Van Hoosen Apartments, Room Only, Quad':
                return 25;
            case "William's Hall, Single, Room Only":
                return 25;
            case "William's Hall, Double, Room Only":
                return 25;
            case "William's Hall, Single, Meal Plan":
                return 25;
            case "William's Hall, Double, Meal Plan":
                return 25;
            case 'University Village Apartments':
                return 17;
            case '1855 Place':
                switch (subHousing) {
                    case 'Studio':
                        return 17;
                    case 'Two Bedroom':
                        return 17;
                    case 'Two Bedroom Efficiency':
                        return 17;
                    case 'Four Bedroom Flat':
                        return 17;
                    case 'Four Bedroom Efficiency':
                        return 17;
                    case 'Four Bedroom Townhouse':
                        return 17;
                    case 'Family Apartments, One Bedroom':
                        return 3;
                    case 'Family Apartments, Two Bedroom':
                        return 3;
                    default:
                        return 0;
                };
            default:
                return 0;
        }
    }

    function GetCreditCount(credits, level, isAgroTech) {
        if ((level === 'Freshman' ||
            level === 'Sophmore' ||
            level === 'Junior' ||
            level === 'Senior') && isAgroTech !== true) {

            if (credits < 12) {
                return credits;
            }

            if (credits >= const_BlockCreditMin && credits <= const_BlockCreditMax) {
                return 15;
            }

            if (credits > 18) {
                return 15 + (credits - 18)
            }
        } else if (level === 'Medical Professional') {
            //all medical professionals have just one rate, the block rate, regardless the number of credits
            return 1;
        }
        else {
            return credits;
        }
    }

    function GetPerHourRate(residency, level, college, isVetClinicalYear) {

        var collegeType = GetCollegeType(college);

        switch (residency) {
            case 'In-state':
                switch (level) {
                    case 'Freshman':
                        if (collegeType === 'professional') return const_InStateProfessionalFreshman;
                        if (collegeType === 'business') return const_InStateBusinessFreshman;
                        if (collegeType === 'engineering') return const_InStateEngineeringFreshman;
                        return const_InStateFreshman;
                    case 'Sophmore':
                        if (collegeType === 'professional') return const_InStateProfessionalSophmore;
                        if (collegeType === 'business') return const_InStateBusinessSophmore;
                        if (collegeType === 'engineering') return const_InStateEngineeringSophmore;
                        return const_InStateSophmore;
                    case 'Junior':
                        if (collegeType === 'professional') return const_InStateProfessionalJunior;
                        if (collegeType === 'business') return const_InStateBusinessJunior;
                        if (collegeType === 'engineering') return const_InStateEngineeringJunior;
                        return const_InStateJunior;
                    case 'Senior':
                        if (collegeType === 'professional') return const_InStateProfessionalSenior;
                        if (collegeType === 'business') return const_InStateBusinessSenior;
                        if (collegeType === 'engineering') return const_InStateEngineeringSenior;
                        return const_InStateSenior;
                    case 'Masters':
                        if (college === 'College of Education') return const_InStateEducationGraduate;
                        if (college === 'College of Engineering') return const_InStateEngineeringGraduate;
                        if (college === 'M.S. Accounting') return const_InStateAccountingGraduate;
                        return const_InStateMasters;
                    case 'Doctoral':
                        if (college === 'College of Education') return const_InStateEducationGraduate;
                        if (college === 'College of Engineering') return const_InStateEngineeringGraduate;
                        if (college === 'M.S. Accounting') return const_InStateAccountingGraduate;
                        return const_InStateDoctoral;
                    case 'Medical Professional':
                        if (college === 'Human Medicine') return const_InStateHumanMedicine;
                        if (college === 'Osteopathic Medicine') return const_InStateOsteopathicMedicine;
                        if (college === 'Veterinary Medicine' && isVetClinicalYear === true) return const_InStateVeternaryMedicineClinical;
                        if (college === 'Veterinary Medicine' && isVetClinicalYear === false) return const_InStateVeternaryMedicineNonClinical;
                        return 0;
                    case 'English Language Center':
                        return const_InStateEnglishLanguageCenter;
                    case 'Teacher Certification Intern':
                        return const_InStateTeacherCertification;
                    case 'Lifelong Student':
                        return const_InStateLifeLongEducation;
                    default:
                        return 0;
                }
                break;
            default:
                switch (level) {
                    case 'Freshman':
                        if (collegeType === 'professional') return const_OutStateProfessionalFreshman;
                        if (collegeType === 'business') return const_OutStateBusinessFreshman;
                        if (collegeType === 'engineering') return const_OutStateEngineeringFreshman;
                        return const_OutStateFreshman;
                    case 'Sophmore':
                        if (collegeType === 'professional') return const_OutStateProfessionalSophmore;
                        if (collegeType === 'business') return const_OutStateBusinessSophmore;
                        if (collegeType === 'engineering') return const_OutStateEngineeringSophmore;
                        return const_OutStateSophmore;
                    case 'Junior':
                        if (collegeType === 'professional') return const_OutStateProfessionalJunior;
                        if (collegeType === 'business') return const_OutStateBusinessJunior;
                        if (collegeType === 'engineering') return const_OutStateEngineeringJunior;
                        return const_OutStateJunior;
                    case 'Senior':
                        if (collegeType === 'professional') return const_OutStateProfessionalSenior;
                        if (collegeType === 'business') return const_OutStateBusinessSenior;
                        if (collegeType === 'engineering') return const_OutStateEngineeringSenior;
                        return const_OutStateSenior;
                    case 'Masters':
                        if (college === 'College of Education') return const_OutStateEducationGraduate;
                        if (college === 'College of Engineering') return const_OutStateEngineeringGraduate;
                        if (college === 'M.S. Accounting') return const_OutStateAccountingGraduate;
                        return const_OutStateMasters;
                    case 'Doctoral':
                        if (college === 'College of Education') return const_OutStateEducationGraduate;
                        if (college === 'College of Engineering') return const_OutStateEngineeringGraduate;
                        if (college === 'M.S. Accounting') return const_OutStateAccountingGraduate;
                        return const_OutStateDoctoral;
                    case 'Medical Professional':
                        if (college === 'Human Medicine') return const_OutStateHumanMedicine;
                        if (college === 'Osteopathic Medicine') return const_OutStateOsteopathicMedicine;
                        if (college === 'Veterinary Medicine' && isVetClinicalYear === true) return const_OutStateVeternaryMedicineClinical;
                        if (college === 'Veterinary Medicine' && isVetClinicalYear === false) return const_OutStateVeternaryMedicineNonClinical;
                        return 0;
                    case 'English Language Center':
                        return const_OutStateEnglishLanguageCenter;
                    case 'Teacher Certification Intern':
                        return const_OutStateTeacherCertification;
                    case 'Lifelong Student':
                        return const_OutStateLifeLongEducation;
                    default:
                        return 0;
                }
        }



    }

    /*
    * I'd hoped to reduce the calculation of tuition to a simple credits * hourly rate forumula; however, the block rate isn't merely the hourly rate * 15.
    * The block rate is close to credits * hourly rate but for Freshman and Sophmores the block tuition is 50 cents higher and for Juniors and Seniors it
    * is 25 cents lower.  This adjustment addresses that little quirk.
    */
    function GetBlockAdjustment(residency, level, credits) {
        if (credits > 11 && residency !== "In-state") {
            switch (level) {
                case 'Freshman':
                case 'Sophmore':
                    return 0.50;
                case 'Junior':
                case 'Senior':
                    return -0.25;
                default:
                    return 0;
            }
        } else {
            return 0;
        }
    }

    /*
        * Colleges are translated into a 'type' which is used in:
        * GetPerHourRate()
        * GetMajorFee()
    */
    function GetCollegeType(college) {

        switch (college) {
            case 'Broad College of Business':
                return 'business';
            case 'College of Agriculture and Natural Resources':
                return 'professional';
            case 'College of Arts and Letters':
                return 'core';
            case 'College of Communication Arts and Sciences':
                return 'core';
            case 'College of Education':
                return 'professional';
            case 'College of Engineering':
                return 'engineering';
            case 'College of Human Medicine':
                return 'professional';
            case 'College of Music':
                return 'professional';
            case 'College of Natural Science':
                return 'core';
            case 'College of Nursing':
                return 'professional';
            case 'College of Osteopathic Medicine':
                return 'professional';
            case 'College of Social Science':
                return 'core';
            case 'College of Veterinary Medicine':
                return 'professional';
            case 'James Madison College':
                return 'core';
            case 'Lyman Briggs College':
                return 'core';
            case 'Neighbor Student Success Collaborative (NSSC)':
                return 'core';
            case 'Residential College in Arts and Humanities':
                return 'core';
            default:
                return 'core';
        }
    }

    function GetMajorType(college, major) {
        /* early exit conditions */
        if (college === 'College of Engineering' || college === 'Broad College of Business') {
            return 'other';
        }

        switch (major) {
            case "Dean's Office":
            case 'Animal Science':
            case 'Biosystems & Ag Engineering':
            case 'Crop & Soil Science':
            case 'Entomology':
            case 'Fisheries & Wildlife':
            case 'Forestry':
            case 'Horticulture':
            case 'Packaging':
            case 'Plant Pathology':
            case 'Communicative Science & Disorders':
            case 'Kinesiology ':
            case 'Biochemistry & Molecular Biology':
            case 'Biological Sci Prog - Interdepartmental':
            case 'Plant Biology':
            case 'Chemistry':
            case 'Entomology':
            case 'Geological Sciences':
            case 'Microbiology & Molecular Genetics':
            case 'Physics-Astronomy':
            case 'Physiology':
            case 'Zoology':
            case 'Lyman Briggs School':
                return 'science';
            case 'Biomedical Lab Diagnostics Program':
            case 'Nursing (traditional)':
            case 'Veterinary Technology (Vet Technology Major Only)':
                return 'health';
            default:
                return 'other';
        }
    }
