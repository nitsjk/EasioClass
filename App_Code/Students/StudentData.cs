using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasioCore
{
    public class StudentData
    {
            //Students Table Info Here
            public long StudentID { get; set; }
            public string StudentName { get; set; }
            public Nullable<System.DateTime> DOB { get; set; }
            public Nullable<System.DateTime> BOA { get; set; }
            public string FathersName { get; set; }
            public string FathersQualification { get; set; }
            public string FathersJob { get; set; }
            public string MothersName { get; set; }
            public string MothersQualification { get; set; }
            public string MothersJob { get; set; }
            public string PresentAddress { get; set; }
            public string PerminantAddress { get; set; }
            public string SessionOfAdmission { get; set; }
            public string PhoneNo { get; set; }
            public string Gender { get; set; }
            public Nullable<bool> Discharged { get; set; }
            public Nullable<long> TransportFee { get; set; }
            public string Phoneno2 { get; set; }
            public string landlineno { get; set; }
            public Nullable<long> StudentFeeRebate { get; set; }
            public Nullable<System.DateTime> WithdrawDate { get; set; }
            public Nullable<int> oldnewmigrated { get; set; }
            public Nullable<long> Ledgerid { get; set; }
            public Nullable<long> Saadhaarcard { get; set; }
            public Nullable<long> Faadhaarcard { get; set; }
            public Nullable<long> Maadhaarcard { get; set; }
            public string BankName { get; set; }
            public string AccountNo { get; set; }
            public string AccountType { get; set; }
            public string IFCCode { get; set; }
            public string Scategory { get; set; }
            public Nullable<long> ScategoryID { get; set; }
            public Nullable<int> BPLStatus { get; set; }
            public string SDisability { get; set; }
            public string Pincode { get; set; }
            public string Religion { get; set; }
            public string MotherTounge { get; set; }
            public string BloodGroup { get; set; }
            public string SEmail { get; set; }
            public string DateOfBirth { get; set; }


            //Student Info Table Properties starts here
            public long StudentInfoID { get; set; }
            public Nullable<long> SIDFK { get; set; }
            public Nullable<long> AdmissionNo { get; set; }
            public string Current_Session { get; set; }
            public Nullable<long> SessionID { get; set; }
            public Nullable<long> ClassID { get; set; }
            public Nullable<long> SectionID { get; set; }
            public Nullable<long> RouteID { get; set; }
            public Nullable<long> StreamID { get; set; }
            public string FeeBookNo { get; set; }
            public string BusFeeBookNo { get; set; }
            public string Remarks { get; set; }
            public string BoardNo { get; set; }
            public string Optional { get; set; }
            public string PrePrimaryBoardNo { get; set; }
            public Nullable<System.DateTime> PrePrimaryDate { get; set; }
            public string PrimaryBoardNo { get; set; }
            public Nullable<System.DateTime> PrimaryDate { get; set; }
            public string MiddleBoardNo { get; set; }
            public Nullable<System.DateTime> MiddleDate { get; set; }
            public string HighBoardNo { get; set; }
            public Nullable<System.DateTime> HighDate { get; set; }
            public string HigherBoardNo { get; set; }
            public Nullable<System.DateTime> HigherDate { get; set; }
            public byte[] Photo { get; set; }
            public string PhotoPath { get; set; }
            public string BusRoute { get; set; }
            public Nullable<long> OptionalID { get; set; }
            public Nullable<long> Rollno { get; set; }
            public Nullable<long> BusStopID { get; set; }
            public string SiblingID { get; set; }
            public Nullable<int> SiblingType { get; set; }
        }
    }
