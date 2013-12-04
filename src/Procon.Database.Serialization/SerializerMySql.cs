using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Procon.Database.Serialization.Builders;
using Procon.Database.Serialization.Builders.Equalities;
using Procon.Database.Serialization.Builders.FieldTypes;
using Procon.Database.Serialization.Builders.Logicals;
using Procon.Database.Serialization.Builders.Methods;
using Procon.Database.Serialization.Builders.Modifiers;
using Procon.Database.Serialization.Builders.Statements;
using Procon.Database.Serialization.Builders.Values;
using Nullable = Procon.Database.Serialization.Builders.Modifiers.Nullable;

namespace Procon.Database.Serialization {
    /// <summary>
    /// Serializer for MySQL support.
    /// </summary>
    public class SerializerMySql : SerializerSql {

        /// <summary>
        /// Parses an index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual String ParseIndex(Index index) {
            String parsed = String.Empty;

            Primary primary = index.FirstOrDefault(attribute => attribute is Primary) as Primary;
            Unique unique = index.FirstOrDefault(attribute => attribute is Unique) as Unique;

            List<String> fields = this.ParseSortings(index);

            // PRIMARY KEY (`Name`)
            if (primary != null) {
                parsed = String.Format("PRIMARY KEY ({0})", String.Join(", ", fields.ToArray()));
            }
            // UNIQUE INDEX `Score_UNIQUE` (`Score` ASC)
            else if (unique != null) {
                parsed = String.Format("UNIQUE INDEX `{0}` ({1})", index.Name, String.Join(", ", fields.ToArray()));
            }
            // INDEX `Name_INDEX` (`Name` ASC)
            else {
                parsed = String.Format("INDEX `{0}` ({1})", index.Name, String.Join(", ", fields.ToArray()));
            }

            return parsed;
        }

        /// <summary>
        /// Parses the list of indexes 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseIndices(IDatabaseObject query) {
            return query.Where(statement => statement is Index).Select(index => this.ParseIndex(index as Index)).ToList();
        }

        /// <summary>
        /// Formats the database name.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        protected virtual String ParseDatabase(Builders.Database database) {
            return String.Format("`{0}`", database.Name);
        }

        /// <summary>
        /// Parses the list of databases.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseDatabases(IDatabaseObject query) {
            return query.Where(statement => statement is Builders.Database).Select(database => this.ParseDatabase(database as Builders.Database)).ToList();
        }

        /// <summary>
        /// Fetches the method from the type of query 
        /// </summary>
        /// <param name="method"></param>
        protected virtual List<string> ParseMethod(Method method) {
            List<string> parsed = new List<string>();

            if (method is Find) {
                parsed.Add("SELECT");
            }
            else if (method is Save || method is Merge) {
                parsed.Add("INSERT");
            }
            else if (method is Create) {
                parsed.Add("CREATE");
            }
            else if (method is Modify) {
                parsed.Add("UPDATE");
            }
            else if (method is Remove) {
                parsed.Add("DELETE");
            }
            else if (method is Drop) {
                parsed.Add("DROP");
            }

            return parsed;
        }

        protected virtual String ParseCollection(Collection collection) {
            return String.Format("`{0}`", collection.Name);
        }

        protected virtual String ParseSort(Sort sort) {
            Collection collection = sort.FirstOrDefault(statement => statement is Collection) as Collection;

            List<String> parsed = new List<String> {
                collection == null ? String.Format("`{0}`", sort.Name) : String.Format("`{0}`.`{1}`", collection.Name, sort.Name)
            };

            if (sort.Any(attribute => attribute is Descending)) {
                parsed.Add("DESC");
            }

            return String.Join(" ", parsed.ToArray());
        }

        protected virtual String ParseType(Builders.FieldType type) {
            List<String> parsed = new List<String>();

            Length length = type.FirstOrDefault(attribute => attribute is Length) as Length;
            Unsigned unsigned = type.FirstOrDefault(attribute => attribute is Unsigned) as Unsigned;

            if (type is StringType) {
                parsed.Add(String.Format("VARCHAR({0})", length == null ? 255 : length.Value));
            }
            else if (type is IntegerType) {
                parsed.Add("INT");

                if (unsigned != null) {
                    parsed.Add("UNSIGNED");
                }
            }

            parsed.Add(type.Any(attribute => attribute is Nullable) == true ? "NULL" : "NOT NULL");

            if (type.Any(attribute => attribute is AutoIncrement) == true) {
                parsed.Add("AUTO INCREMENT");
            }

            return String.Join(" ", parsed.ToArray());
        }

        protected virtual String ParseField(Field field) {
            List<String> parsed = new List<String>();

            if (field.Any(attribute => attribute is Distinct)) {
                parsed.Add("DISTINCT");
            }

            Collection collection = field.FirstOrDefault(statement => statement is Collection) as Collection;

            parsed.Add(collection == null ? String.Format("`{0}`", field.Name) : String.Format("`{0}`.`{1}`", collection.Name, field.Name));

            if (field.Any(attribute => attribute is Builders.FieldType)) {
                parsed.Add(this.ParseType(field.First(attribute => attribute is Builders.FieldType) as Builders.FieldType));
            }

            return String.Join(" ", parsed.ToArray());
        }

        protected virtual String ParseEquality(Equality equality) {
            String parsed = "";

            if (equality is Equals) {
                parsed = "=";
            }
            else if (equality is GreaterThan) {
                parsed = ">";
            }
            else if (equality is GreaterThanEquals) {
                parsed = ">=";
            }
            else if (equality is LessThan) {
                parsed = "<";
            }
            else if (equality is LessThanEquals) {
                parsed = "<=";
            }

            return parsed;
        }

        /// <summary>
        /// Escapes a value of a string 
        /// @todo implement properly
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual String EscapeString(String value) {
            return value;
        }

        protected virtual String ParseValue(Value value) {
            String parsed = "";

            NumericValue numeric = value as NumericValue;
            StringValue @string = value as StringValue;
            RawValue raw = value as RawValue;

            if (numeric != null) {
                if (numeric.Integer.HasValue == true) {
                    parsed = numeric.Integer.Value.ToString(CultureInfo.InvariantCulture);
                }
                else if (numeric.Float.HasValue == true) {
                    parsed = numeric.Float.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
            else if (@string != null) {
                parsed = String.Format(@"""{0}""", this.EscapeString(@string.Data));
            }
            else if (raw != null) {
                parsed = raw.Data;
            }

            return parsed;
        }

        protected virtual List<String> ParseFields(IDatabaseObject query) {
            List<String> parsed = new List<String>();

            // If we have a distinct field, but no specific fields
            if (query.Any(attribute => attribute is Distinct) == true && query.Any(logical => logical is Field) == false) {
                parsed.Add("DISTINCT *");
            }
            // If we have a distinct field and only one field available
            else if (query.Any(attribute => attribute is Distinct) == true && query.Count(logical => logical is Field) == 1) {
                Field field = query.First(logical => logical is Field) as Field;

                if (field != null) {
                    field.Add(query.First(attribute => attribute is Distinct));

                    parsed.Add(this.ParseField(field));
                }
            }
            // Else, no distinct in the global query space
            else {
                parsed.AddRange(query.Where(logical => logical is Field).Select(field => this.ParseField(field as Field)));
            }

            return parsed;
        }

        /// <summary>
        /// Parse collections for all methods, used like "FROM ..."
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseCollections(IDatabaseObject query) {
            return query.Where(logical => logical is Collection).Select(collection => this.ParseCollection(collection as Collection)).ToList();
        }

        protected virtual List<String> ParseEqualities(IDatabaseObject query) {
            List<String> equalities = new List<String>();

            foreach (Equality equality in query.Where(statement => statement is Equality)) {
                Field field = equality.FirstOrDefault(statement => statement is Field) as Field;
                Value value = equality.FirstOrDefault(statement => statement is Value) as Value;

                if (field != null && value != null) {
                    equalities.Add(String.Format("{0} {1} {2}", this.ParseField(field), this.ParseEquality(equality), this.ParseValue(value)));
                }
            }

            return equalities;
        }

        /// <summary>
        /// Parse any logicals 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseLogicals(IDatabaseObject query) {
            List<String> logicals = new List<String>();

            foreach (Logical logical in query.Where(logical => logical is Logical)) {
                String separator = logical is Or ? " OR " : " AND ";

                String childLogicals = String.Join(separator, this.ParseLogicals(logical).ToArray());
                String childEqualities = String.Join(separator, this.ParseEqualities(logical).ToArray());

                if (String.IsNullOrEmpty(childLogicals) == false) logicals.Add(String.Format("({0})", childLogicals));
                if (String.IsNullOrEmpty(childEqualities) == false) logicals.Add(String.Format("({0})", childEqualities));
            }

            return logicals;
        }

        /// <summary>
        /// Parse the conditions of a SELECT, UPDATE and DELETE query. Used like "SELECT ... "
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseConditions(IDatabaseObject query) {
            List<String> conditions = this.ParseLogicals(query);

            conditions.AddRange(this.ParseEqualities(query));

            return conditions;
        }

        /// <summary>
        /// Parse field assignments, similar to conditions, but without the conditionals.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseAssignments(IDatabaseObject query) {
            List<String> assignments = new List<String>();

            foreach (Assignment assignment in query.Where(statement => statement is Assignment)) {
                Field field = assignment.FirstOrDefault(statement => statement is Field) as Field;
                Value value = assignment.FirstOrDefault(statement => statement is Value) as Value;

                if (field != null && value != null) {
                    assignments.Add(String.Format("{0} = {1}", this.ParseField(field), this.ParseValue(value)));
                }
            }

            return assignments;
        }

        protected virtual List<String> ParseSortings(IDatabaseObject query) {
            return query.Where(sort => sort is Sort).Select(sort => this.ParseSort(sort as Sort)).ToList();
        }

        public override ICompiledQuery Compile(IParsedQuery parsed) {
            CompiledQuery serializedQuery = new CompiledQuery() {
                Children = parsed.Children.Select(this.Compile).ToList(),
                Root = parsed.Root
            };

            List<String> compiled = new List<String>() {
                parsed.Methods.FirstOrDefault()
            };

            if (parsed.Root is Merge) {
                ICompiledQuery save = serializedQuery.Children.FirstOrDefault(child => child.Root is Save);
                ICompiledQuery modify = serializedQuery.Children.FirstOrDefault(child => child.Root is Modify);

                if (save != null && modify != null) {
                    compiled.Add("INTO");
                    compiled.Add(save.Collections);
                    compiled.Add("SET");
                    compiled.Add(save.Assignments);
                    compiled.Add("ON DUPLICATE KEY UPDATE");
                    compiled.Add(modify.Assignments);
                }
            }
            else if (parsed.Root is Find) {
                serializedQuery.Fields = new List<String>(parsed.Fields);
                compiled.Add(parsed.Fields.Any() == true ? String.Join(", ", parsed.Fields.ToArray()) : "*");

                if (parsed.Collections.Any() == true) {
                    serializedQuery.Collections = String.Join(", ", parsed.Collections.ToArray());
                    compiled.Add("FROM");
                    compiled.Add(serializedQuery.Collections);
                }

                if (parsed.Conditions.Any() == true) {
                    serializedQuery.Conditions = String.Join(" AND ", parsed.Conditions.ToArray());
                    compiled.Add("WHERE");
                    compiled.Add(serializedQuery.Conditions);
                }

                if (parsed.Sortings.Any() == true) {
                    serializedQuery.Sortings = String.Join(", ", parsed.Sortings.ToArray());
                    compiled.Add("ORDER BY");
                    compiled.Add(serializedQuery.Sortings);
                }
            }
            else if (parsed.Root is Create) {
                if (parsed.Databases.Any() == true) {
                    serializedQuery.Databases = parsed.Databases.FirstOrDefault();
                    compiled.Add("DATABASE");
                    compiled.Add(serializedQuery.Databases);
                }
                else if (parsed.Collections.Any() == true) {
                    compiled.Add("TABLE");
                    compiled.Add(parsed.Collections.FirstOrDefault());

                    if (parsed.Indices.Any() == true) {
                        List<String> fieldsIndicesCombination = new List<String>(parsed.Fields);
                        fieldsIndicesCombination.AddRange(parsed.Indices);

                        serializedQuery.Indices = String.Join(", ", fieldsIndicesCombination.ToArray());

                        compiled.Add(String.Format("({0})", serializedQuery.Indices));
                    }
                    else {
                        compiled.Add(String.Format("({0})", String.Join(", ", parsed.Fields.ToArray())));
                    }
                }
            }
            else if (parsed.Root is Save) {
                if (parsed.Collections.Any() == true) {
                    compiled.Add("INTO");
                    serializedQuery.Collections = parsed.Collections.FirstOrDefault();
                    compiled.Add(serializedQuery.Collections);
                }

                if (parsed.Assignments.Any() == true) {
                    serializedQuery.Assignments = String.Join(", ", parsed.Assignments.ToArray());
                    compiled.Add("SET");
                    compiled.Add(serializedQuery.Assignments);
                }
            }
            else if (parsed.Root is Modify) {
                if (parsed.Collections.Any() == true) {
                    serializedQuery.Collections = String.Join(", ", parsed.Collections.ToArray());
                    compiled.Add(serializedQuery.Collections);
                }

                if (parsed.Assignments.Any() == true) {
                    serializedQuery.Assignments = String.Join(", ", parsed.Assignments.ToArray());
                    compiled.Add("SET");
                    compiled.Add(serializedQuery.Assignments);
                }

                if (parsed.Conditions.Any() == true) {
                    serializedQuery.Conditions = String.Join(" AND ", parsed.Conditions.ToArray());
                    compiled.Add("WHERE");
                    compiled.Add(serializedQuery.Conditions);
                }

            }
            else if (parsed.Root is Remove) {
                if (parsed.Collections.Any() == true) {
                    serializedQuery.Collections = String.Join(", ", parsed.Collections.ToArray());
                    compiled.Add("FROM");
                    compiled.Add(serializedQuery.Collections);
                }

                if (parsed.Conditions.Any() == true) {
                    serializedQuery.Conditions = String.Join(" AND ", parsed.Conditions.ToArray());
                    compiled.Add("WHERE");
                    compiled.Add(serializedQuery.Conditions);
                }
            }
            else if (parsed.Root is Drop) {
                if (parsed.Databases.Any() == true) {
                    serializedQuery.Databases = parsed.Databases.FirstOrDefault();
                    compiled.Add("DATABASE");
                    compiled.Add(serializedQuery.Databases);
                }
                else if (parsed.Collections.Any() == true) {
                    compiled.Add("TABLE");
                    serializedQuery.Collections = parsed.Collections.FirstOrDefault();
                    compiled.Add(serializedQuery.Collections);
                }
            }

            serializedQuery.Completed = String.Join(" ", compiled.ToArray());

            return serializedQuery;
        }

        public override ISerializer Parse(Method method, IParsedQuery parsed) {
            parsed.Root = method;

            parsed.Children = this.ParseChildren(method);

            parsed.Methods = this.ParseMethod(method);

            parsed.Databases = this.ParseDatabases(method);

            parsed.Indices = this.ParseIndices(method);

            parsed.Fields = this.ParseFields(method);

            parsed.Assignments = this.ParseAssignments(method);

            parsed.Conditions = this.ParseConditions(method);

            parsed.Collections = this.ParseCollections(method);

            parsed.Sortings = this.ParseSortings(method);

            return this;
        }
    }
}
